namespace INSS.EIIR.Models.ExtractModels
{
    public class ExtractAvailable
    {    
        public int ExtractId { get; set; }
        public DateTime Currentdate { get; set; }
        public string SnapshotCompleted { get; set; }
        public DateTime? SnapshotDate { get; set; }
        public string ExtractCompleted { get; set; }
        public DateTime? ExtractDate { get; set; }
        public int? ExtractEntries { get; set; }
        public int? ExtractBanks { get; set; }
        public int? ExtractIvas { get; set; }
        public int? NewCases { get; set; }
        public int? NewBanks { get; set; }
        public int? NewIvas { get; set; }
        public string ExtractFilename { get; set; }
        public string DownloadLink { get; set; }
        public string DownloadZiplink { get; set; }
        public int? ExtractDros { get; set; }
        public int? NewDros { get; set; }
    }
}
