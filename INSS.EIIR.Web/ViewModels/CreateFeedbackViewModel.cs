using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Web.ViewModels;

public class CreateFeedbackViewModel
{
    public CreateFeedbackViewModel()
    {
        CaseFeedback = new CreateCaseFeedback();
        ErrorListParameters = new ErrorListParameters();
        SearchParameters = new SearchParameters();
    }

    public int CaseNo { get; set; }

    public int IndivNo { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public bool FromAdmin { get; set; }

    public DateTime ArrangementDate { get; set; }

    public CreateCaseFeedback CaseFeedback { get; set; }

    public SearchParameters SearchParameters { get; set; }

    public ErrorListParameters ErrorListParameters { get; set; }

    public IEnumerable<BreadcrumbLink> Breadcrumbs { get; set; }
}