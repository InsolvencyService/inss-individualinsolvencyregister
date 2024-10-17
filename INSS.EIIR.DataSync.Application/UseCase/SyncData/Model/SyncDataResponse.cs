using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Model
{
    public class SyncDataResponse
    {
        public int ErrorCount { get; set; }
        
        public bool IsError { get { return ErrorCount > 0; } }

        public string ErrorMessage { get; set; }     
    }
}
