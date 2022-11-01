using INSS.EIIR.Models.Breadcrumb;
using System.Diagnostics.CodeAnalysis;

namespace INSS.EIIR.Models.Home
{
    [ExcludeFromCodeCoverage]
    public class StaticContent
    {
        public IEnumerable<BreadcrumbLink>? Breadcrumbs { get; set; }
    }
}