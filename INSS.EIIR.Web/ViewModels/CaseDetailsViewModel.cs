using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Web.ViewModels;

public class CaseDetailsViewModel
{
    public CaseResult CaseDetails { get; set; }

    public SearchParameters SearchParameters { get; set; }

    public ErrorListParameters ErrorListParameters { get; set; }

    public IEnumerable<BreadcrumbLink> Breadcrumbs { get; set; }
}