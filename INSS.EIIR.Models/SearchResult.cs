using System.Diagnostics.CodeAnalysis;

namespace INSS.EIIR.Models;

[ExcludeFromCodeCoverage]
public class SearchResult
{
    public string Title { get; set; }
    
    public string FirstName { get; set; }

    public string Surname { get; set; } 
    
    public int CaseNo { get; set; }

    public int IndivNo { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string Address3 { get; set; }

    public string Wflag { get; set; }

    public string Court { get; set; }

    public string Number { get; set; }

    public string DRONumber { get; set; }

    public DateTime? DateOfOrder { get; set; }

    public int? Year { get; set; }

    public string Type { get; set; }

    public int Office { get; set; }
}