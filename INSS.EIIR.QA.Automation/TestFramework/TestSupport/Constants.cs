using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INSS.EIIR.QA.Automation.TestFramework.Hooks;

namespace INSS.EIIR.QA.Automation.TestFramework.TestSupport
{
    public static class Constants
    {
        public static string EIIRBaseUrl => WebDriverFactory.Config["BaseUrl"];
    }
}
