using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.AzureSearch.IndexMapper
{
    public class FailedToGetIndexException : Exception
    {
        public FailedToGetIndexException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
