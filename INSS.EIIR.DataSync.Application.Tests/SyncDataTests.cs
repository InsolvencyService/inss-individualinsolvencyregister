using INSS.EIIR.DataSync.Application.Tests.FakeData;
using INSS.EIIR.DataSync.Application.Tests.TestDoubles;
using INSS.EIIR.DataSync.Application.UseCase.SyncData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.Extensions.Logging;
using NSubstitute;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Exceptions;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;
using NSubstitute.ExceptionExtensions;
using INSS.EIIR.Models.SyncData;
using Azure;

namespace INSS.EIIR.DataSync.Application.Tests
{
    public class SyncDataTests
    {
        [Fact]
        public async Task Given_data_SyncData_sinks()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create().ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false })).Build();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)   
                .WithLogger(logger)
                .Build();

            // act
            await sut.Handle(new SyncDataRequest() 
            { 
                Modes = Models.Constants.SyncData.Mode.Default, 
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.Received().Sink(Arg.Is(rec));
            await dataSink.Received().Complete(true);
            logger.DidNotReceive().Log(default, default, default, default, default, default);
        }

        [Fact]
        public async Task Given_invalid_data_SyncData_sinks_failure_and_not_data()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(InvalidData.NegativeId()).Build();
            var dataSink = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithValidationRule(new IdValidationRule())
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            });

            // assert
            await failureSink.Received().Sink(Arg.Any<SyncFailure>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            // assert that commit => false on validation error
            await dataSink.DidNotReceive().Complete(true);
            logger.ReceivedWithAnyArgs().LogError(default, default, default, default);            
        }

        [Fact]
        public async Task Given_sinkError_SyncData_does_not_commit_sink()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(ValidData.Standard()).Build();
            var dataSink = MockDataSinkBuilder.Create().ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = true })).Build();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.DidNotReceive().Complete(true);
        }

        [Fact]
        public async Task Given_transformationError_SyncData_sinks_failure_and_not_data()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(ValidData.Standard()).Build();
            var dataSink = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var transformRule = MockDataTransformRuleBuilder.Create().ThatReturns(Task.FromResult(new TransformRuleResponse() { IsError = true })).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithTransformationRule(transformRule)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            });

            // assert
            await failureSink.Received().Sink(Arg.Any<SyncFailure>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            // assert that commit => false on transformation error
            await dataSink.DidNotReceive().Complete(true);
            logger.ReceivedWithAnyArgs().LogError(default, default, default, default);
        }

        [Fact]
        public async Task Given_precondtion_failure_SyncData_sinks_failure_returns_error_response()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(ValidData.Standard()).Build();
            var dataSink = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "N" }).Build();
            var transformRule = MockDataTransformRuleBuilder.Create().ThatReturns(Task.FromResult(new TransformRuleResponse() { IsError = true })).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithTransformationRule(transformRule)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            var response = await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            });

            // assert
            await failureSink.Received().Sink(Arg.Any<SyncFailure>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            // assert that commit => false on transformation error
            await dataSink.DidNotReceive().Complete(true);
            Assert.Equal(1, response.ErrorCount);
        }

        [Fact]
        public async Task Given_tranformation_rule_throws_exception_SyncData_throws_TransformException()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(ValidData.Standard()).Build();
            var dataSink = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var transformRule = MockDataTransformRuleBuilder.Create().ThatReturns(Task.FromException<TransformRuleResponse>(new Exception("john was here"))).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithTransformationRule(transformRule)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act & assert
            await Assert.ThrowsAsync<TransformRuleException>(async() => await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            }));

        }

        [Fact]
        public async Task Given_validation_rule_throws_exception_SyncData_throws_ValidationRuleException()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(ValidData.Standard()).Build();
            var dataSink = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var transformRule = MockDataTransformRuleBuilder.Create().ThatReturns(Task.FromResult(new TransformRuleResponse() { IsError = true })).Build();
            var validationRule = MockDataValidationRuleBuilder.Create().ThatReturns(Task.FromException<ValidationRuleResponse>(new Exception("john was here"))).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithTransformationRule(transformRule)
                .WithValidationRule(validationRule)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act & assert
            await Assert.ThrowsAsync<ValidationRuleException>(async () => await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            }));

        }


        [Fact]
        public async Task Given_failureSink_throws_exception_SyncData_throws_DataSinkException()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(ValidData.Standard()).Build();
            var dataSink = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var transformRule = MockDataTransformRuleBuilder.Create().ThatReturns(Task.FromResult(new TransformRuleResponse() { IsError = true })).Build();
            var validationRule = MockDataValidationRuleBuilder.Create().ThatReturns(Task.FromResult(new ValidationRuleResponse() { IsValid = true })).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>(); 

            failureSink.Sink(Arg.Any<SyncFailure>()).Throws(new Exception());

            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithTransformationRule(transformRule)
                .WithValidationRule(validationRule)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act & assert
            await Assert.ThrowsAsync<DataSinkException>(async () => await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            }));

        }

        [Fact]
        public async Task Given_a_dataSink_throws_exception_SyncData_throws_DataSinkException()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(ValidData.Standard()).Build();
            var dataSink = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();

            dataSink.Sink(Arg.Any<InsolventIndividualRegisterModel>()).Throws(new Exception());

            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var transformRule = MockDataTransformRuleBuilder.Create().ThatReturns(Task.FromResult(new TransformRuleResponse() { IsError = false })).Build();
            var validationRule = MockDataValidationRuleBuilder.Create().ThatReturns(Task.FromResult(new ValidationRuleResponse() { IsValid = true })).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();

            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithTransformationRule(transformRule)
                .WithValidationRule(validationRule)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act & assert
            await Assert.ThrowsAsync<DataSinkException>(async () => await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            }));

        }


        [Fact]
        public async Task Given_requestdataSourceIsNone_SyncData_sinks_failure_returns_error_response()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create().ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false })).Build();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var logger = Substitute.For<ILogger<SyncData>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            var response = await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.None
            });

            // assert
            await failureSink.Received().Sink(Arg.Any<SyncFailure>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.DidNotReceive().Complete(true);
            Assert.Equal(1, response.ErrorCount);
        }

        [Fact]
        public async Task Given_requestdataSourceIsNotValid_SyncData_sinks_failure_returns_error_response()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create().ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false })).Build();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var logger = Substitute.For<ILogger<SyncData>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            var response = await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.Default,
                DataSources = Models.Constants.SyncData.Datasource.FakeDRO
            });

            // assert
            await failureSink.Received().Sink(Arg.Any<SyncFailure>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.DidNotReceive().Complete(true);
            Assert.Equal(1, response.ErrorCount);
        }

        [Fact]
        public async Task Given_precondtions_ignored_SyncData_sinks_record_completes()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create().ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false })).Build();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "N" }).Build();
            var transformRule = MockDataTransformRuleBuilder.Create().ThatReturns(Task.FromResult(new TransformRuleResponse() { IsError = false })).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithTransformationRule(transformRule)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            var response = await sut.Handle(new SyncDataRequest()
            {
                Modes = Models.Constants.SyncData.Mode.IgnorePreConditionChecks,
                DataSources = Models.Constants.SyncData.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.Received().Sink(Arg.Is(rec));
            await dataSink.Received().Complete(true);
        }

    }
}