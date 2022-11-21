namespace INSS.EIIR.Web.Constants;

public static class FeedbackFilters
{
    public static Dictionary<int, string> OrganisationFilters = new()
    {
        {0, "All"},
        {1, "Member of the public"},
        {2, "Credit reference agency"},
        {3, "Credit card issuer"},
        {4, "Bank or building society"},
        {5, "Mortgage provider"},
        {6, "Government department"},
        {7, "Financial services"},
        {8, "Debt recovery agency"},
        {9, "Other"}
    };

    public static Dictionary<string, string> InsolvencyTypeFilters = new()
    {
        {"A", "All"},
        {"B", "Bankruptcies"},
        {"D", "Debt relief orders"},
        {"I", "IVAs"}
    };

    public static Dictionary<int, string> StatusFilters = new()
    {
        {0, "All"},
        {1, "Not viewed"},
        {2, "Viewed"}
    };
}