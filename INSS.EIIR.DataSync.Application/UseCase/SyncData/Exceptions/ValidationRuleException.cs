namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Exceptions
{
    public class ValidationRuleException : Exception
    {
        public ValidationRuleException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
