using INSS.EIIR.Models.Breadcrumb;

namespace INSS.EIIR.Web.ViewModels
{
    public class ModifyBannerMessageViewModel
    {
        public string BannerText { get; set; }

        public IEnumerable<BreadcrumbLink> Breadcrumbs { get; set; }
    }
}
