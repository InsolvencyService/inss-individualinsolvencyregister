using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;
using NSubstitute;

namespace INSS.EIIR.DataSync.Application.Tests.TestDoubles
{
    public class MockDataValidationRuleBuilder
    {

        private Task<ValidationRuleResponse> _validationResponse;

        public static MockDataValidationRuleBuilder Create() { return new MockDataValidationRuleBuilder(); }


        public MockDataValidationRuleBuilder ThatReturns(Task<ValidationRuleResponse> taskResponse)
        {
            _validationResponse = taskResponse;

            return this;
        }

        public IValidationRule Build()
        {
            var mock = Substitute.For<IValidationRule>();

            mock.Validate(Arg.Any<InsolventIndividualRegisterModel>()).Returns(_validationResponse);

            return mock;
        }

    }
}
