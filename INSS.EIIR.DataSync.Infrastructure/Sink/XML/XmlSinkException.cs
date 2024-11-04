namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class XmlSinkException : Exception
    {
        public XmlSinkException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
