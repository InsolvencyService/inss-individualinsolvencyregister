namespace INSS.EIIR.Models.FeedbackModels;

public class ErrorListParameters
{
    public int Page { get; set; }

    public string InsolvencyType { get; set; }

    public int Organisation { get; set; }

    public int Status { get; set; }

    public int CaseNo { get; set; }

    public int IndivNo { get; set; }
}