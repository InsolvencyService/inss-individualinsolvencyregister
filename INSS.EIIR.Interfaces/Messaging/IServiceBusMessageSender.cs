namespace INSS.EIIR.Interfaces.Messaging
{
    public interface IServiceBusMessageSender
    {
        Task SendMessageAsync<T>(T message, string queueName, IDictionary<string, object> applicationProperties = null);
    }
}
