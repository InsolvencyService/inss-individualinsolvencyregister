using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Web.ViewModels;

public class SubscriberListViewModel
{
    public SubscriberWithPaging SubscriberWithPaging { get; set; }

    public IEnumerable<BreadcrumbLink> Breadcrumbs { get; set; }
}