using System.Diagnostics.CodeAnalysis;

namespace INSS.EIIR.Models.CaseModels;

[ExcludeFromCodeCoverage]
public class CaseRequest
{

    public int CaseNo { get; set; }

    public int IndivNo { get; set; }
}