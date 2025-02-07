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
using Azure;
using INSS.EIIR.Models.SyncData;

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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
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
            var dataSink = MockDataSinkBuilder.Create().ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false })).Build();
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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
            });

            // assert
            await failureSink.DidNotReceive().Sink(Arg.Any<SyncFailure>());
            await dataSink.Received().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            // assert that commit => false on validation error
            await dataSink.Received().Complete(true);
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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
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
            await Assert.ThrowsAsync<TransformRuleException>(async () => await sut.Handle(new SyncDataRequest()
            {
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
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
                Modes = SyncDataEnums.Mode.EnableValidations,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.None
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
                Modes = SyncDataEnums.Mode.Default,
                DataSources = SyncDataEnums.Datasource.FakeDRO
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
                Modes = SyncDataEnums.Mode.IgnorePreConditionChecks,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.Received().Sink(Arg.Is(rec));
            await dataSink.Received().Complete(true);
        }

        [Fact]
        public async Task Given_XMLDisabled_SyncData_doesnot_sink()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create()
                .ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false }))
                .ThatHasPropertyEnabledCheckBit(SyncDataEnums.Mode.DisableXMLExtract)
                .Build();
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
                Modes = SyncDataEnums.Mode.DisableXMLExtract,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.DidNotReceive().Start(Arg.Any<SyncDataEnums.Datasource>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.DidNotReceive().Complete(Arg.Any<bool>());
        }

        [Fact]
        public async Task Given_RebuildIndexDisabled_SyncData_doesnot_sink()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create()
                .ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false }))
                .ThatHasPropertyEnabledCheckBit(SyncDataEnums.Mode.DisableIndexRebuild)
                .Build();
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
                Modes = SyncDataEnums.Mode.DisableIndexRebuild,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.DidNotReceive().Start(Arg.Any<SyncDataEnums.Datasource>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.DidNotReceive().Complete(Arg.Any<bool>());
        }

        [Fact]
        public async Task Given_RebuildIndexDisabled_SyncData_XML_sinks()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create()
                .ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false }))
                .ThatHasPropertyEnabledCheckBit(SyncDataEnums.Mode.DisableXMLExtract)
                .Build();
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
                Modes = SyncDataEnums.Mode.DisableIndexRebuild,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.Received().Start(Arg.Any<SyncDataEnums.Datasource>());
            await dataSink.Received().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.Received().Complete(Arg.Any<bool>());
        }

        [Fact]
        public async Task Given_XMLDisabled_SyncData_IndexRebuild_sinks()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create()
                .ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false }))
                .ThatHasPropertyEnabledCheckBit(SyncDataEnums.Mode.DisableIndexRebuild)
                .Build();
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
                Modes = SyncDataEnums.Mode.DisableXMLExtract,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.Received().Start(Arg.Any<SyncDataEnums.Datasource>());
            await dataSink.Received().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.Received().Complete(Arg.Any<bool>());
        }


        [Fact]
        public async Task Given_TestModeEnable_SyncData_data_sinks_butcommitFalse()
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).Build();
            var dataSink = MockDataSinkBuilder.Create()
                .ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false }))
                .ThatHasPropertyEnabledCheckBit(SyncDataEnums.Mode.DisableIndexRebuild)
                .Build();
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
                Modes = SyncDataEnums.Mode.Test,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
            });

            // assert
            await dataSink.Received().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.Received().Complete(false);
        }

        [Theory]
        [InlineData(SyncDataEnums.Datasource.FakeBKTandIVA, SyncDataEnums.Datasource.FakeDRO)]
        [InlineData(SyncDataEnums.Datasource.FakeBKTandIVA|SyncDataEnums.Datasource.IscisDRO, SyncDataEnums.Datasource.FakeDRO)]
        [InlineData(SyncDataEnums.Datasource.FakeBKTandIVA, SyncDataEnums.Datasource.FakeDRO|SyncDataEnums.Datasource.IscisDRO)]
        public async Task Given_TestModeEnabled_PermittedDSIssue_SyncData_data_sinks_butcommitFalse(SyncDataEnums.Datasource permitted, SyncDataEnums.Datasource specified)
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).ThatIsA(permitted).Build();
            var dataSource2 = MockDataSourceBuilder.Create().ThatHas(rec).ThatIsA(SyncDataEnums.Datasource.FakeDRO).Build();
            var dataSource3 = MockDataSourceBuilder.Create().ThatHas(rec).ThatIsA(SyncDataEnums.Datasource.IscisDRO).Build();
            var dataSink = MockDataSinkBuilder.Create()
                .ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false }))
                .ThatHasPropertyEnabledCheckBit(SyncDataEnums.Mode.DisableIndexRebuild)
                .Build();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSource(dataSource2)
                .WithDataSource(dataSource3)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithLogger(logger)
                .WithPermittedDataSource(permitted)
                .Build();

            // act
            await sut.Handle(new SyncDataRequest()
            {
                Modes = SyncDataEnums.Mode.Test,
                DataSources = specified
            });

            // assert
            await dataSink.Received().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.Received().Complete(false);
        }

        [Theory]
        [InlineData(SyncDataEnums.Datasource.FakeBKTandIVA, SyncDataEnums.Datasource.FakeDRO)]
        [InlineData(SyncDataEnums.Datasource.FakeBKTandIVA|SyncDataEnums.Datasource.IscisDRO, SyncDataEnums.Datasource.FakeDRO)]
        [InlineData(SyncDataEnums.Datasource.FakeBKTandIVA, SyncDataEnums.Datasource.FakeDRO|SyncDataEnums.Datasource.IscisDRO)]
        public async Task Given_PermittedDSIssue_SyncData_data_does_not_sinks_or_complete(SyncDataEnums.Datasource permitted, SyncDataEnums.Datasource specified)
        {
            // arrange
            var rec = ValidData.Standard();
            var dataSource = MockDataSourceBuilder.Create().ThatHas(rec).ThatIsA(permitted).Build();
            var dataSource2 = MockDataSourceBuilder.Create().ThatHas(rec).ThatIsA(SyncDataEnums.Datasource.FakeDRO).Build();
            var dataSource3 = MockDataSourceBuilder.Create().ThatHas(rec).ThatIsA(SyncDataEnums.Datasource.IscisDRO).Build();
            var dataSink = MockDataSinkBuilder.Create()
                .ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = false }))
                .ThatHasPropertyEnabledCheckBit(SyncDataEnums.Mode.DisableIndexRebuild)
                .Build();
            var extractRepo = MockDataExtractRepositoryBuilder.Create().ThatReturns(new Extract() { ExtractCompleted = "N", SnapshotCompleted = "Y" }).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSource(dataSource2)
                .WithDataSource(dataSource3)
                .WithDataSink(dataSink)
                .WithExtractRepo(extractRepo)
                .WithLogger(logger)
                .WithPermittedDataSource(permitted)
                .Build();

            // act
            await sut.Handle(new SyncDataRequest()
            {
                Modes = SyncDataEnums.Mode.Default,
                DataSources = specified
            });

            // assert
            await dataSink.DidNotReceive().Start(Arg.Any<SyncDataEnums.Datasource>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            await dataSink.DidNotReceive().Complete(Arg.Any<bool>());
        }

        [Fact]
        public async Task Given_ValidationRuleReturns_false_ValidationsEnabled_SyncData_does_not_complete()
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
                Modes = SyncDataEnums.Mode.EnableValidations,
                DataSources = SyncDataEnums.Datasource.FakeBKTandIVA
            });

            // assert
            await failureSink.Received().Sink(Arg.Any<SyncFailure>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            // assert that commit => false on validation error
            await dataSink.DidNotReceive().Complete(true);
            logger.ReceivedWithAnyArgs().LogError(default, default, default, default);
        }

    }

}