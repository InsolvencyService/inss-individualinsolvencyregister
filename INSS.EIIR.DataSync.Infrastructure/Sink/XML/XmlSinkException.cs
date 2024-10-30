using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class XmlSinkException : Exception
    {
        public XmlSinkException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
