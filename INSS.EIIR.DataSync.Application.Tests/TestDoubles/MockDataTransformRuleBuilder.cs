using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Service;
using NSubstitute;

namespace INSS.EIIR.DataSync.Application.Tests.TestDoubles
{
    public class MockDataTransformRuleBuilder
    {

        private Task<TransformRuleResponse> _transformResponse;

        public static MockDataTransformRuleBuilder Create() { return new MockDataTransformRuleBuilder(); }


        public MockDataTransformRuleBuilder ThatReturns(Task<TransformRuleResponse> taskResponse)
        {
            _transformResponse = taskResponse;

            return this;
        }

        public ITransformRule Build()
        {
            var mock = Substitute.For<ITransformRule>();

            mock.Transform(Arg.Any<InsolventIndividualRegisterModel>()).Returns(_transformResponse);

            return mock;
        }
    }
}