namespace INSS.EIIR.Web.Helper
{
    public static class DateTimeExtensions
    {
        public static bool IsValidDate(this string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString) && string.IsNullOrEmpty(dateString))
                return false;

            return DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result);
        }
    }
}
