namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class ExistingBankruptciesOptions
    {
        public string BlobStorageConnectionString { get; set; }

        public string BlobStorageContainer { get; set; }

        public string ExistingBankruptciesFileName { get; set; }
    }
}