using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INSS.EIIR.Models.CaseModels;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Model
{
    public class InsolventIndividualRegisterModel : CaseResult
    {

        public string Id { get => caseNo.ToString(); }

        public string annulDate { get; set; }
        
        public string annulReason { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is InsolventIndividualRegisterModel model)
            {
                return this.caseNo.Equals(model.caseNo) &&
                    this.individualForenames.Equals(model.individualForenames) &&
                    this.individualSurname.Equals(model.individualSurname);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.caseNo, this.individualForenames, this.individualSurname);
        }
    }
}
