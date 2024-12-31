using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INSS.EIIR.Models.SyncData
{

    public class SyncDataRequest
    {
        public Constants.SyncData.Mode Modes { get; set; }

        public Constants.SyncData.Datasource DataSources { get; set; }
    }
}
