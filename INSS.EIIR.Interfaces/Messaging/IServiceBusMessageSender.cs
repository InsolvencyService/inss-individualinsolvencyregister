namespace INSS.EIIR.Interfaces.Messaging
{
    public interface IServiceBusMessageSender
    {
        Task SendExtractJobMessageAsync<T>(T message, string queueName, IDictionary<string, object> applicationProperties = null);
        Task SendNotifyMessageAsync<T>(T message, string queueName, IDictionary<string, object> applicationProperties = null);
    }
}
