
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    public class RestrictionValidationRule : IValidationRule
    {
        public async Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model)
        {
            bool isValid = true;
            var errorMessage = "";

            if (model.restrictionsStartDate != null)
            {
                if (model.hasRestrictions == false || model.restrictionsType == null || model.restrictionsType == "")
                {
                    isValid = false;
                    errorMessage = $"case {model.caseNo} when restrictionsStartDate has a value the following conditons should hold; hasRestriction = true and restrictionsType must contain either: Order, Undertaking or Interim Order";
                }
            }

            if (model.hasRestrictions == true)
            {
                if (model.restrictionsType == null || model.restrictionsType == "" || model.restrictionsStartDate == null)
                {
                    isValid = false;
                    errorMessage = $"case {model.caseNo} when hasRestrictions is true the following conditons should hold; restrictionsStartDate should contain a value, restrictionsType must contain either: Order, Undertaking or Interim Order";
                }
            }

            if (model.restrictionsType != null && model.restrictionsType != "")
            {
                if (model.hasRestrictions == false  || model.restrictionsStartDate == null)
                {
                    isValid = false;
                    errorMessage = $"case {model.caseNo} when restrictionType has a value the following conditons should hold; hasRestriction = true, restrictionsStartDate should contain a value";
                }
            }

            return new ValidationRuleResponse()
            {
                IsValid = isValid,
                ErrorMessage = errorMessage
            };
        }


    }
}
