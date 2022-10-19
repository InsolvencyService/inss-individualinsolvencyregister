using INSS.EIIR.Models.Breadcrumb;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Models.Home
{
    [ExcludeFromCodeCoverage]
    public class StaticContent
    {
        public IEnumerable<BreadcrumbLink>? Breadcrumbs { get; set; }
    }
}
