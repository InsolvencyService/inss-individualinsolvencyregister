using Azure.Messaging.ServiceBus;
using INSS.EIIR.Interfaces.Messaging;
using Microsoft.Extensions.Azure;
using System.Text.Json;

namespace INSS.EIIR.Services
{
    public class ServiceBusMessageSender : IServiceBusMessageSender
    {
        private readonly ServiceBusClient _serviceBusClient;

        public ServiceBusMessageSender(
            IAzureClientFactory<ServiceBusClient> serviceBusClientFactory)
        {
            _serviceBusClient = serviceBusClientFactory.CreateClient("ServiceBusPublisher");
        }

        public async Task SendMessageAsync<T>(T message, string queueName, IDictionary<string, object> applicationProperties = null)
        {
            if (string.IsNullOrEmpty(queueName)) throw new ArgumentNullException(nameof(queueName));

            await using var sender = _serviceBusClient.CreateSender(queueName);
            try
            {
                var jsonEntity = JsonSerializer.Serialize(message);
                var serviceBusMessage = new ServiceBusMessage(jsonEntity);

                if (applicationProperties?.Count > 0)
                {
                    foreach (var kvp in applicationProperties)
                    {
                        serviceBusMessage.ApplicationProperties.Add(kvp);
                    }
                }
                await sender.SendMessageAsync(serviceBusMessage);
            }
            finally
            {
                await sender.CloseAsync();
            }
        }
    }
}