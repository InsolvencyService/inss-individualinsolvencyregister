using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    public class RestrictionEndDateValidationRule : IValidationRule
    {
        public async Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model)
        {
            bool isValid = true;
            var errorMessage = "";

            if (model.hasRestrictions)
            {
                if (model.restrictionsEndDate != null && model.restrictionsEndDate < DateTime.Now)
                { 
                    isValid = false;
                    errorMessage = $"case {model.caseNo} restriction has expired";
                }
            }
            else 
            {
                if (model.restrictionsEndDate != null)
                {
                    isValid = false;
                    errorMessage = $"case {model.caseNo} restriction has an enddate when hasRestrictions = false";
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
