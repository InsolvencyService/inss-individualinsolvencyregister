namespace INSS.EIIR.Models.SubscriberModels
{
    public class SubscriberWithPaging
    {
        public PagingModel Paging { get; set; }
        public IEnumerable<Subscriber> Subscribers { get; set; }
    } 
}
