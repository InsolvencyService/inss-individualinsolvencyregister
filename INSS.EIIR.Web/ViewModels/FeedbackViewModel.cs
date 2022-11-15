using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.FeedbackModels;

namespace INSS.EIIR.Web.ViewModels;

public class FeedbackViewModel
{
    public FeedbackWithPaging FeedbackWithPaging { get; set; }

    public IEnumerable<BreadcrumbLink> Breadcrumbs { get; set; }
}