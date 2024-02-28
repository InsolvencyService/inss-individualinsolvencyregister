namespace INSS.EIIR.Models.SubscriberModels
{
    public class SubscriberWithPaging
    {
        public SubscriberWithPaging()
        {
            Subscribers = new List<Subscriber>();
        }

        public PagingModel Paging { get; set; }

        public List<Subscriber> Subscribers { get; set; }
    } 
}
