namespace INSS.EIIR.Data.Models
{
    public partial class SubscriberDownload
    {
        public string ExtractId { get; set; } = null!;
        public string SubscriberId { get; set; } = null!;
        public string DownloadIpaddress { get; set; } = null!;
        public string DownloadWebserver { get; set; } = null!;
        public string ExtractZipdownload { get; set; } = null!;
        public DateTime DownloadDate { get; set; }
    }
}
