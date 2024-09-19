using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Model
{
    public class InsolventIndividualRegisterModel
    {
        public string Id { get; set; }
        public string Forenames { get; set; }
        public string Surname { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is InsolventIndividualRegisterModel model)
            {
                return this.Id.Equals(model.Id) &&
                    this.Forenames.Equals(model.Forenames) &&
                    this.Surname.Equals(model.Surname);
            }
            else return false;
        }
    }
}
