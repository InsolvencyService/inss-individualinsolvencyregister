using INSS.EIIR.Models.TableStorage;

namespace INSS.EIIR.Models.ExtractModels
{
    public class EiirExtractNotification : AzureTableStorage
    {
        public string SubscriberId { get; set; }
        public string Filename { get; set; }
        public string EmailAddress { get; set; }
        public DateTime Sent { get; set; }
    }
}
