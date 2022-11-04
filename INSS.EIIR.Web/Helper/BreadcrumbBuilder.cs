using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.SubscriberModels;
using INSS.EIIR.Web.ViewModels;

namespace INSS.EIIR.Web.Helper
{
    public static class BreadcrumbBuilder
    {
        public static IList<BreadcrumbLink> BuildBreadcrumbs(
            bool showSearch = false,
            bool showSubscriberList = false,
            bool showSubscriber = false,
            SubscriberParameters subscriberParameters = null)
        {
            var breadcrumbs = new List<BreadcrumbLink> {
                new() { Text = "Home", Href = "/" },
            };

            if (showSearch)
            {
                breadcrumbs.Add(new BreadcrumbLink { Text = "Search", Href = "/search" });
            }

            if (showSubscriberList)
            {
                var parameters = string.Empty;
                if (subscriberParameters != null)
                {
                    parameters = $"/{subscriberParameters.Page}/{subscriberParameters.Active}";
                }

                breadcrumbs.Add(new BreadcrumbLink { Text = "Subscribers", Href = $"/admin/subscribers" + parameters });
            }

            if (showSubscriber)
            {
                var parameters = string.Empty;
                if (subscriberParameters != null)
                {
                    parameters = $"/{subscriberParameters.SubscriberId}/{subscriberParameters.Page}/{subscriberParameters.Active}";
                }

                breadcrumbs.Add(new BreadcrumbLink { Text = "Subscriber details", Href = $"/admin/subscriber{parameters}" });
            }

            return breadcrumbs;
        }
    }
}