using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace INSS.EIIR.Models.CaseModels;

[ExcludeFromCodeCoverage]
public class CaseResult
{

    [Key]
    public int caseNo { get; set; }

    public int indivNo { get; set; }

    public string? indvidualForenames { get; set; }
    public string? indvidualSurname { get; set; }

    public string? indvidualTitle { get; set; }

    public string? indvidualGender { get; set; }

    public DateTime? indvidualDOB { get; set; }

    public string? indvidualOccupation { get; set; }
    public string? indvidualAddress { get; set; }

    public string? indvidualPostcode { get; set; }

    public string? indvidualAddressWithheld { get; set; }

    public string? indvidualAlias { get; set; }

    public string? caseName { get; set; }
    public string? courtName { get; set; }

    public string? insolvencyType { get; set; }

    public DateTime? notificationDate { get; set; }

    public string? caseStatus { get; set; }

    public DateTime? insolvencyDate { get; set; }

    public string? insolvencyPractitionerName { get; set; }

    public string? insolvencyPractitionerFirmName { get; set; }
    public string? insolvencyPractitionerAddress { get; set; }

    public string? insolvencyPractitionerPostcode { get; set; }

    public string? insolvencyPractitionerTelephone { get; set; }

    public string? insolvencyServiceOffice { get; set; }

    public string? insolvencyServiceContact { get; set; }
    public string? insolvencyServiceAddress { get; set; }

    public string? insolvencyServicePostcode { get; set; }
    public string? insolvencyServicePhone { get; set; }

}