namespace INSS.EIIR.Models.Configuration
{
    public class ServiceBusConfig
    {
        public string PublisherConnectionString { get; set; } = null!;
        public string ExtractJobQueue { get; set; } = null!;
        public string NotifyQueue { get; set; } = null!;
    }
}
