using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation
{
    internal interface IValidationRule
    {
        Task<ValidationRuleResponse> Validate(InsolventIndividualRegisterModel model);
    }
}
