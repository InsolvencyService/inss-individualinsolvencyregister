using INSS.EIIR.Data.Models;

namespace INSS.EIIR.Interfaces.DataAccess
{
    public interface IExtractRepository
    {
        ExtractAvailabilitySP GetExtractAvailability();
        void UpdateExtractAvailability();
        Task<IList<SubscriberAccount>> GetActiveSubscribers();
        Task<SubscriberApplication> GetSubscriberDetails(string subscriberId);
    }
}
