using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Models.Breadcrumb
{
    [ExcludeFromCodeCoverage]
    public class BreadcrumbLink
    {
        public string Href { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;
    }
}
