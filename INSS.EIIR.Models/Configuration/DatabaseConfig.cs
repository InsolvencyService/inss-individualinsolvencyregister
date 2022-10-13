namespace INSS.EIIR.Models.Configuration
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; } = null!;
        public int CommandTimeout { get; set; }
        public long DataBufferSize { get; set; }
        public string GetXmlDataProcedure { get; set; } = null!;
        public string GetExtractAvailableProcedure { get; set; } = null!;
        public string UpdateExtractAvailableProcedure { get; set; } = null!;
    }
}
