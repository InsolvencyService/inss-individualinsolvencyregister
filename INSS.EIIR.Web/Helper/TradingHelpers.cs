using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSS.EIIR.Web.Helper
{
    public static class TradingHelpers
    {
        public static string removeXMLFromCompanyName(string companyName) {

            companyName = companyName.Replace("<TradingName>", "");
            companyName = companyName.Replace("<TradingAddress>", "");
            companyName = companyName.Replace("</TradingName>", "</br>");
            companyName = companyName.Replace("</TradingAddress>", "</br>");
            
            return companyName;
        }
    }
}