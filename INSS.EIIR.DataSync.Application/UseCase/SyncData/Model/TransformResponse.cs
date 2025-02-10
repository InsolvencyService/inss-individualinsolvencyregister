using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Model
{
    public class TransformResponse
    {
        public InsolventIndividualRegisterModel Model { get; set; }
        public bool IsError { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
