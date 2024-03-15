using System.Globalization;
namespace INSS.EIIR.Web.Extensions;

public static class StringExtensions
{
    public static string ToTitleCase(this string s) =>
        CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());

    public static bool IsValidDate(this string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString) && string.IsNullOrEmpty(dateString))
            return false;

        return DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result);
    }
}