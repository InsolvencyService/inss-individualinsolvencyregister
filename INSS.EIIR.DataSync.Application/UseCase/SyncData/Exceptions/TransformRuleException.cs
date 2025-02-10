namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Exceptions
{
    public class TransformRuleException : Exception
    {
        public TransformRuleException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
