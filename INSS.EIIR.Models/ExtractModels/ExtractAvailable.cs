namespace INSS.EIIR.Models.ExtractModels
{
    public class ExtractAvailable
    {
        public ExtractAvailable(int extractId, DateTime currentdate, string snapshotCompleted, DateTime? snapshotDate, string extractCompleted, DateTime? extractDate, int? extractEntries, int? extractBanks, int? extractIvas, int? newCases, int? newBanks, int? newIvas, string extractFilename, string downloadLink, string downloadZiplink, int? extractDros, int? newDros)
        {
            ExtractId = extractId;
            Currentdate = currentdate;
            SnapshotCompleted = snapshotCompleted;
            SnapshotDate = snapshotDate;
            ExtractCompleted = extractCompleted;
            ExtractDate = extractDate;
            ExtractEntries = extractEntries;
            ExtractBanks = extractBanks;
            ExtractIvas = extractIvas;
            NewCases = newCases;
            NewBanks = newBanks;
            NewIvas = newIvas;
            ExtractFilename = extractFilename;
            DownloadLink = downloadLink;
            DownloadZiplink = downloadZiplink;
            ExtractDros = extractDros;
            NewDros = newDros;
        }
    
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
