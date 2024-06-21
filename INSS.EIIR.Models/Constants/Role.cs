using static Azure.Core.HttpHeader;

namespace INSS.EIIR.Models.Constants;

public class Role
{
    public const string Admin = "Admin";
    public const string Subscriber = "Subscriber";
}

public static class InsolvencyType
{
    public const string BANKRUPTCY = "Bankruptcy";
    public const string IVA = "Individual Voluntary Arrangement";
    public const string DRO = "Debt Relief Order";
}

public static class RestrictionsType
{
    public const string ORDER = "Order";
    public const string INTERIMORDER = "Interim Order";
    public const string UNDERTAKING = "Undertaking";
}

public static class Common
{
    public const string NoOtherNames = "No OtherNames Found";
    public const string NoLastKnownTown = "No Last Known Town Found";
    public const string NoLastKnownPostCode = "No Last Known PostCode Found";
    public const string NoTradingNames = "<No Trading Names Found>";

}