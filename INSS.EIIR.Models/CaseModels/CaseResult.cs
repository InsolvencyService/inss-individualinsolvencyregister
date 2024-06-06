using INSS.EIIR.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

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
    public string? individualTown { get; set; }

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

    //Properties which supports Restrictions (IBRO,BRO,BRU for Bankruptcys) and (DRRO,DRRU for Debt Relief Orders)
    public bool hasRestrictions { get; set; }

    //Possile values null, Interim Order, Order, Undertaking
    public string restrictionsType { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? restrictionsStartDate { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? restrictionsEndDate { get; set; }

    //Applies in practice to BROs only an individual may be subject to an IBRO before a BRO
    public bool hasaPrevInterimRestrictionsOrder { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? prevInterimRestrictionsOrderStartDate { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? prevInterimRestrictionsOrderEndDate { get; set; }

    //Properties which support Insolvency Practitioner details
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
    public string? deceasedDate { get; set; }

    public DateTime? dateOfPreviousOrder { get; set; }
    [NotMapped]
    public Trading? Trading { get; set; }
    [NotMapped]
    public IIRRecordType RecordType {
        get 
        {
            switch (insolvencyType) {

                case InsolvencyType.BANKRUPTCY:
                    if (hasRestrictions && restrictionsType == RestrictionsType.ORDER)
                        return IIRRecordType.BRO;
                    else
                        return IIRRecordType.BKT;
                case InsolvencyType.IVA:
                    return IIRRecordType.IVA;
                case InsolvencyType.DRO:
                    return IIRRecordType.DRO;
                default:
                    throw new Exception ("Undefined Insolvency Type");
            }       
        }    
    }

}

[XmlRoot("Trading")]
public class Trading
{
    [XmlElement("TradingDetails")]
    public List<TradingDetails> TradingDetails { get; set; }
}

public class TradingDetails
{
    [XmlElement("TradingName")]
    public string TradingName { get; set; }
    [XmlElement("TradingAddress")]
    public string TradingAddress { get; set; }
}

public enum IIRRecordType
{
    //Bankruptcy - lasts for 1 year
    BKT, 
    //Bankruptcy Restrictions Undertaking - MAY follow a BKT if indivdual voluntarily agrees
    BRU,
    //Bankruptcy Restrictions Order - MAY follow BKT if enforced by court
    BRO,
    //Interim Bankruptcy Restrictions Order - MAY follow BKT, before a BRO
    IBRO,
    //Debt Relief Order
    DRO,
    //Debt Relief Restrictions Order - MAY follow a DRO if indivdual voluntarily agrees
    DRRO,
    //Debt Relief Restrictions Undertaking - MAY follow a DRO if enforced
    DRRU,
    //Interim Debt Relief Restrictions Order - these practically do not exist
    IDRRO,
    //Individual Volunarty Arrangement
    IVA
}