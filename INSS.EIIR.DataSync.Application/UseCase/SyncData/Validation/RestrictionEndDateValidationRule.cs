using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
