using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Interfaces.AzureSearch;

public interface IIndiviualSearchFilter
{
    string ApplyFilter(IndividualSearchModel searchModel);
}