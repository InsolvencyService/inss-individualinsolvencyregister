using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.AISearch
{
    public class FailedToCreateIndexException : Exception
    {
        public FailedToCreateIndexException(string message, Exception innerException) : base(message, innerException) { }
    }
}
