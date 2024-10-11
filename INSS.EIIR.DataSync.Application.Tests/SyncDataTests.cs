using Castle.Core.Logging;
using INSS.EIIR.DataSync.Application.Tests.FakeData;
using INSS.EIIR.DataSync.Application.Tests.TestDoubles;
using INSS.EIIR.DataSync.Application.UseCase.SyncData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.Extensions.Logging;
using NSubstitute;

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
            await sut.Handle();

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
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            await sut.Handle();

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
            await sut.Handle();

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
            await sut.Handle();

            // assert
            await failureSink.Received().Sink(Arg.Any<SyncFailure>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            // assert that commit => false on transformation error
            await dataSink.DidNotReceive().Complete(true);
            logger.ReceivedWithAnyArgs().LogError(default, default, default, default);
        }



    }
}