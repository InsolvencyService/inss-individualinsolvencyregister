using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Service
{
    public class ValidationService
    {
        private readonly List<IValidationRule> _rules = new List<IValidationRule>();

        public ValidationService() 
        {
            // create instances of all validation rules..
            Type[] iValidationRuleTypes = (from t in Assembly.GetExecutingAssembly().GetExportedTypes()
                                 where !t.IsInterface && !t.IsAbstract
                                 where typeof(IValidationRule).IsAssignableFrom(t)
                                 select t).ToArray();
            _rules = iValidationRuleTypes.Select(t => (IValidationRule)Activator.CreateInstance(t)).ToList();
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
