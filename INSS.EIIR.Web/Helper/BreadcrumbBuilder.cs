using INSS.EIIR.Models.Breadcrumb;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Models.SearchModels;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Web.Helper
{
    public static class BreadcrumbBuilder
    {
        public static IList<BreadcrumbLink> BuildBreadcrumbs(
            bool showSearch = false,
            bool showSearchList = false,
            bool showSearchDetails = false,
            bool showSubscriberList = false,
            bool showSubscriber = false,
            bool showErrorList = false,
            bool isAdmin = false,
            SubscriberParameters subscriberParameters = null,
            SearchParameters searchParameters = null,
            ErrorListParameters errorListParameters = null)
        {
            var breadcrumbs = new List<BreadcrumbLink> {
                new() { Text = "Home", Href = !isAdmin ? "/" : "/admin/admin-area" },
            };

            if (showSearch)
            {
                breadcrumbs.Add(new BreadcrumbLink { Text = "Search the Individual Insolvency Register", Href = "/search" });
            }

            if (showSearchList)
            {
                var parameters = string.Empty;
                if (searchParameters != null)
                {
                    parameters = $"/{searchParameters.SearchTerm}/{searchParameters.Page}";
                }

                breadcrumbs.Add(new BreadcrumbLink { Text = "Search results", Href = "/search-results" + parameters });
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

                breadcrumbs.Add(new BreadcrumbLink
                {
                    Text = "Subscriber details",
                    Href = $"/admin/subscriber{parameters}"
                });
            }

            if (showErrorList)
            {
                var parameters = string.Empty;
                if (errorListParameters != null)
                {
                    parameters = $"/{errorListParameters.Page}/{errorListParameters.InsolvencyType}/{errorListParameters.Organisation}/{errorListParameters.Status}";
                }

                breadcrumbs.Add(new BreadcrumbLink
                {
                    Text = "Errors or issues",
                    Href = $"/admin/errors-or-issues{parameters}"
                });
            }

            if (showSearchDetails)
            {
                var parameters = string.Empty;

                if (isAdmin)
                {
                    if(errorListParameters != null)
                    {
                        parameters =
                            $"/{errorListParameters.Page}/true/{errorListParameters.CaseNo}/{errorListParameters.IndivNo}/a/{errorListParameters.InsolvencyType}/{errorListParameters.Organisation}/{errorListParameters.Status}";
                    }
                }
                else
                {
                    if (searchParameters != null)
                    {
                        parameters =
                            $"/{searchParameters.Page}/false/{searchParameters.CaseNo}/{searchParameters.IndivNo}/{searchParameters.SearchTerm}";
                    }
                }

                breadcrumbs.Add(new BreadcrumbLink
                {
                    Text = "Case details",
                    Href = $"/case-details{parameters}"
                });
            }

            return breadcrumbs;
        }
    }
}