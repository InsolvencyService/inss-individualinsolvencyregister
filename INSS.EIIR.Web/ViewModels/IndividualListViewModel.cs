using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Web.ViewModels;

public class IndividualListViewModel
{
    public SearchResults SearchResults { get; set; }

    public IEnumerable<BreadcrumbLink> Breadcrumbs { get; set; }
}