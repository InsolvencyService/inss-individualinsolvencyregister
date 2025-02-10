using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Service
{
    public class ValidationService
    {
        private readonly IEnumerable<IValidationRule> _rules;

        public ValidationService(IEnumerable<IValidationRule> rules)
        {
            _rules = rules;
        }

        public async Task<ValidationResponse> Validate(InsolventIndividualRegisterModel model)
        {
            var validationResponse = new ValidationResponse()
            {
                Model = model,
                ErrorMessages = new List<string>(),
                IsValid = true
            };

            foreach (var rule in _rules)
            {
                var response = await rule.Validate(model);
                if (!response.IsValid)
                {
                    validationResponse.ErrorMessages.Add(response.ErrorMessage);
                    validationResponse.IsValid = false;
                }
            }

            return validationResponse;
        }
    }
}
