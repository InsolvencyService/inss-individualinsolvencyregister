using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.FeedbackModels;

namespace INSS.EIIR.Web.ViewModels;

public class FeedbackViewModel
{
    public FeedbackWithPaging FeedbackWithPaging { get; set; }

    public IEnumerable<BreadcrumbLink> Breadcrumbs { get; set; }

    public string InsolvencyTypeFilter { get; set; }

    public int OrganisationFilter { get; set; }

    public int StatusFilter { get; set; }
}