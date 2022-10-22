namespace INSS.EIIR.Models.SubscriberModels
{
    public class SubscriberDownloadDetail
    {
        public SubscriberDownloadDetail(int extractId, string iPAddress, string server, string extractZipDownload)
        {
            ExtractId = extractId;
            IPAddress = iPAddress;
            Server = server;
            ExtractZipDownload = extractZipDownload;    
        }

        public int ExtractId { get; set; }  
        public string IPAddress { get; set; }
        public string Server { get; set; }
        public string ExtractZipDownload { get; set; }
    }
}
