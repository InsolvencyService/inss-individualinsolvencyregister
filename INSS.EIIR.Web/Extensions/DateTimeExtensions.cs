namespace INSS.EIIR.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsValidDate(this DateTime? dateString)
        {
            return dateString != null && dateString != DateTime.MinValue;
        }
    }
}
