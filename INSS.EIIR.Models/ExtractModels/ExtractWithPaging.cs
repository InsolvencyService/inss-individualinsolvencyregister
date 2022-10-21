namespace INSS.EIIR.Models.ExtractModels
{
    public class ExtractWithPaging
    {
        public PagingModel Paging { get; set; }
        public IEnumerable<Extract> Extracts { get; set; }
    }
}
