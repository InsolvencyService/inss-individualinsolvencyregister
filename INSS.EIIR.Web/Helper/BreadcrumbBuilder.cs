using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Web.Constants;

namespace INSS.EIIR.Web.Helper
{
    public static class BreadcrumbBuilder
    {
        public static IList<BreadcrumbLink> BuildBreadcrumbs(bool showSearch = false, bool isAdmin = false)
        {
            // add home link in by default
            var breadcrumbs = new List<BreadcrumbLink> {
                new BreadcrumbLink{ Text = "Home", Href = isAdmin ? "/" : string.Concat("/", AreaNames.Admin) },
            };

            // commendted out the FIP implementation for the moment.

            //if (showSearch)
            //{
            //    breadcrumbs.Add(new BreadcrumbLink { Text = "Search", Href = "/IP/Search" });
            //}

            //if (showResults)
            //{
            //    breadcrumbs.Add(new BreadcrumbLink { Text = "Search results", Href = "/IP/Results" });
            //}

            //if (showIp)
            //{
            //    breadcrumbs.Add(new BreadcrumbLink { Text = ipName ?? "Insolvency practitioner", Href = $"/IP/IP/{ipNumber}" });
            //}

            return breadcrumbs;
        }

     }
}
