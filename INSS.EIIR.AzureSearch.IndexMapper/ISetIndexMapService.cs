using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.AzureSearch.IndexMapper
{
    public interface ISetIndexMapService
    {
        Task SetIndexName(string name);
    }
}
