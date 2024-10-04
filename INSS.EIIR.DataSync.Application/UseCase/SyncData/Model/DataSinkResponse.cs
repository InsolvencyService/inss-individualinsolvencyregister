using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Model
{
    public class DataSinkResponse
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
