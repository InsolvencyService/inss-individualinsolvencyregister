
namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public interface IExistingBankruptciesService
    {
        Task<SortedList<int, int>> GetExistingBankruptcies();

        Task SetExistingBankruptcies(SortedList<int, int> existingBankruptcies);
    }

}