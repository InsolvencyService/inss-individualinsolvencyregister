namespace INSS.EIIR.Web.Helper
{
    public static class DateTimeExtensions
    {
        public static bool IsValidDate(this string dateString)
        {
            if (dateString == null || dateString.Length == 0)
                return false;

            return DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result);
        }
    }
}
