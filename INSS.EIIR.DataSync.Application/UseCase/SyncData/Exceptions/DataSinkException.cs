namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Exceptions
{
    public class DataSinkException : Exception
    {
        public DataSinkException(string message, Exception innerException) : base(message, innerException) 
        { }
    }
}
