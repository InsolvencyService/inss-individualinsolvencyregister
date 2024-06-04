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

        //Outputs the number of years and days between two dates for display purposes
        //More than 5 years - output only year portion rounded for the number of days
        //Less than 5 years - both years and days
        //Less than a years - only days
        //Ignores leap years as hopefully good enough for display purposes
        public static string Duration(this DateTime? self, DateTime? to)
        {

            if (self == null || to == null || self > to)
                return "";

            var ts = ((DateTime)to).Subtract((DateTime)self);

            var yearsTxt = "";
            var daysTxt = "";

            var days = (int)ts.Days;
            var years = days / 365;

            if (years != 0)
                days = days - (365 * years);

            var daySffx = days == 1 ? "" : "s";
            var yearSffx = years == 1 ? "" : "s";

            if (years >= 5)
            {
                if (((double)days / 365) > 0.5)
                    years += 1;
                yearsTxt = $"{years} years";
            }
            else if (years >= 1)
            {
                yearsTxt = $"{years} year{yearSffx} ";
                daysTxt = $"{days} day{daySffx}";
            }
            else
            { 
                daysTxt = $"{days} day{daySffx}";
            }

            return $"{yearsTxt}{daysTxt}";
        }

    }
}
