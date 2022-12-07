using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.FeedbackModels;

namespace INSS.EIIR.Web.ViewModels;

public class CreateFeedbackViewModel
{
    public CreateFeedbackViewModel()
    {
        CaseFeedback = new CreateCaseFeedback();
    }

    public int CaseNo { get; set; }

    public int IndivNo { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public DateTime ArrangementDate { get; set; }

    public CreateCaseFeedback CaseFeedback { get; set; }

    public IEnumerable<BreadcrumbLink> Breadcrumbs { get; set; }
}