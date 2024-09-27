using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.AzureSearch.IndexMapper
{
    public class IndexMapperOptions
    {
        public string TableStorageUri { get; set; }
        public string TableStorageAccountName { get; set; }
        public string TableStorageKey { get; set; }
    }
}
