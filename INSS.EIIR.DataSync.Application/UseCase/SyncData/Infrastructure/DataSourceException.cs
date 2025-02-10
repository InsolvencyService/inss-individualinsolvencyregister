using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure
{
    public class DataSourceException : Exception
    {
        public DataSourceException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
