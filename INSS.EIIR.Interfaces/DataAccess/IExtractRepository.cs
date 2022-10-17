using INSS.EIIR.Models.ExtractModels;

namespace INSS.EIIR.Interfaces.DataAccess;

public interface IExtractRepository
{
    ExtractAvailable GetExtractAvailable();
    void UpdateExtractAvailable();
}
