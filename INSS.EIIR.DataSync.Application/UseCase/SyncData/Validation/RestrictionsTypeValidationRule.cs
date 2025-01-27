using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    public class RestrictionsTypeValidationRule : IValidationRule
    {
        public async Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model)
        {
            bool isValid = true;
            var errorMessage = "";

            if (model.restrictionsType != null)
            {
                switch (model.restrictionsType) 
                {
                    case "":
                    case "Order":
                    case "Undertaking":
                    case "Interim Order":
                        break;
                    default:
                        isValid = false;
                        errorMessage = $"case {model.caseNo} restrictionsType '{model.restrictionsType}' unknown";
                        break;
                
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
