using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace INSS.EIIR.Models.CaseModels;

[ExcludeFromCodeCoverage]
public class CaseResult
{

    [Key]
    public int caseNo { get; set; }

    public int indivNo { get; set; }

    public string? individualForenames { get; set; }
    public string? individualSurname { get; set; }

    public string? individualTitle { get; set; }

    public string? individualGender { get; set; }

    public string? individualDOB { get; set; }

    public string? individualOccupation { get; set; }
    public string? individualAddress { get; set; }

    public string? individualPostcode { get; set; }

    public string? individualAddressWithheld { get; set; }

    public string? individualAlias { get; set; }

    public string? caseName { get; set; }
    public string? courtName { get; set; }
    public string? courtNumber { get; set; }
    public string? caseYear { get; set; }

    public string? insolvencyType { get; set; }

    public DateTime? notificationDate { get; set; }
    public string? insolvencyDate { get; set; }

    public string? caseStatus { get; set; }
    public string? caseDescription { get; set; }
    public string? tradingNames { get; set; }

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