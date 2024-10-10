using Castle.Core.Logging;
using INSS.EIIR.DataSync.Application.Tests.FakeData;
using INSS.EIIR.DataSync.Application.Tests.TestDoubles;
using INSS.EIIR.DataSync.Application.UseCase.SyncData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
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
            var logger = Substitute.For<ILogger<SyncData>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
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
            var dataSource = MockDataSourceBuilder.Create().ThatHas(InvalidData.NoId()).Build();
            var dataSink = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            await sut.Handle();

            // assert
            await failureSink.Received().Sink(Arg.Any<SyncFailure>());
            await dataSink.DidNotReceive().Sink(Arg.Any<InsolventIndividualRegisterModel>());
            logger.ReceivedWithAnyArgs().LogError(default, default, default, default);            
        }

        [Fact]
        public async Task Given_sinkError_SyncData_does_not_commit_sink()
        {
            // arrange
            var dataSource = MockDataSourceBuilder.Create().ThatHas(ValidData.Standard()).Build();
            var dataSink = MockDataSinkBuilder.Create().ThatReturns(Task.FromResult(new DataSinkResponse() { IsError = true })).Build();
            var logger = Substitute.For<ILogger<SyncData>>();
            var failureSink = Substitute.For<IDataSink<SyncFailure>>();
            var sut = SyncDataApplicationBuilder.Create()
                .WithDataSource(dataSource)
                .WithDataSink(dataSink)
                .WithFailureSink(failureSink)
                .WithLogger(logger)
                .Build();

            // act
            await sut.Handle();

            // assert
            await dataSink.DidNotReceive().Complete(true);
        }



    }
}