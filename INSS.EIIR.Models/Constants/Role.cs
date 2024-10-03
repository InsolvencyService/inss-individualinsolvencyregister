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

public static class RestrictionsTypeXmlText
{
    public const string BRO = "BANKRUPTCY RESTRICTIONS ORDER (BRO)";
    public const string BRU = "BANKRUPTCY RESTRICTIONS UNDERTAKING (BRU)";
    public const string IBRO = "INTERIM BANKRUPTCY RESTRICTIONS ORDER (IBRO)";
    public const string DRRO = "DEBT RELIEF RESTRICTIONS ORDER (DRRO)";
    public const string DRRU = "DEBT RELIEF RESTRICTION UNDERTAKING (DRRU)";
    public const string PREVIBRONOTE = "This BRO was preceded by an Interim Bankruptcy Restrictions Order (IBRO)";
}

public static class Common
{
    public const string NoOtherNames = "No OtherNames Found";
    public const string NoLastKnownTown = "No Last Known Town Found";
    public const string NoLastKnownPostCode = "No Last Known PostCode Found";
    public const string NoTradingNames = "<No Trading Names Found>";

}