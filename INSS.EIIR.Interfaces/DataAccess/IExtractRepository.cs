using INSS.EIIR.Models.ExtractModels;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.DataAccess
{
    public interface IExtractRepository
    {
        ExtractAvailable GetExtractAvailable();
        void UpdateExtractAvailable();
        Task<IList<Subscriber>> GetActiveSubscribers();
        Task<SubscriberDetail> GetSubscriberDetails(string subscriberId);
    }
}
