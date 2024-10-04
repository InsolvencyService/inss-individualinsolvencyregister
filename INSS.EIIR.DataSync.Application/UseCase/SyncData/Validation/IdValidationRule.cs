using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    public class IdValidationRule : IValidationRule
    {
        public async Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model)
        {
            bool isValid = model.caseNo > -1 ;
            var errorMessage = "";

            if (!isValid) 
            {
                errorMessage = "model identifier is negative";
            }

            return new ValidationRuleResponse()
            {
                IsValid = isValid,
                ErrorMessage = errorMessage
            };
        }
    }
}
