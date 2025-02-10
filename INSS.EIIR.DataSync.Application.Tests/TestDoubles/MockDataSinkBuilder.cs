using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.SyncData;
using NSubstitute;

namespace INSS.EIIR.DataSync.Application.Tests.TestDoubles
{
    public class MockDataSinkBuilder
    {
        private Task<DataSinkResponse> _sinkResponse;
        private SyncDataEnums.Mode _enabledCheckBit;

        public static MockDataSinkBuilder Create() { return new MockDataSinkBuilder(); }

        public MockDataSinkBuilder ThatReturns(Task<DataSinkResponse> model)
        {
            _sinkResponse = model;

            return this;
        }

        public MockDataSinkBuilder ThatHasPropertyEnabledCheckBit(SyncDataEnums.Mode enabledCheckBit)
        {
            _enabledCheckBit = enabledCheckBit;

            return this;
        }

        public IDataSink<InsolventIndividualRegisterModel> Build()
        {
            var mock = Substitute.For<IDataSink<InsolventIndividualRegisterModel>>();

            mock.Sink(Arg.Any<InsolventIndividualRegisterModel>()).Returns(_sinkResponse);
            mock.EnabledCheckBit.Returns(_enabledCheckBit);

            return mock;
        }
    }
}
