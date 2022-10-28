using System.Globalization;

namespace INSS.EIIR.Web.Extensions;

public static class StringExtensions
{
    public static string ToTitleCase(this string s) =>
        CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
}