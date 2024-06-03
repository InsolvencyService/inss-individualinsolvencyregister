using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Net.NetworkInformation;
using System.Security.Cryptography.Pkcs;

namespace INSS.EIIR.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsValidDate(this DateTime? dateString)
        {
            return dateString != null && dateString != DateTime.MinValue;
        }

        //Outputs the number of years and days between two dates
        //More than 5 years - output only year portion rounded for the number of days
        //Less than 5 years - both years and days
        //Less than a years - only days
        public static string Duration(this DateTime? self, DateTime? to)
        {

            if (self == null || to == null || self > to)
                return "";

            var ts = ((DateTime)to).Subtract((DateTime)self);

            var yearstxt = "";
            var daystxt = "";

            var days = (int)ts.Days;
            var years = days / 365;

            if (years != 0)
                days = days - (365 * years);

            var daysfx = days == 1 ? "" : "s";
            var yearsfx = years == 1 ? "" : "s";

            if (years >= 5)
            {
                if (((double)days / 365) > 0.5)
                    years += 1;
                yearstxt = $"{years} years";
            }
            else if (years >= 1)
            {
                yearstxt = $"{years} year{yearsfx} ";
                daystxt = $"{days} day{daysfx}";
            }
            else
            { 
                daystxt = $"{days} day{daysfx}";
            }

            return $"{yearstxt}{daystxt}";
        }

    }
}
