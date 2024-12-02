using INSS.EIIR.Models.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Serialization;

namespace INSS.EIIR.Models.CaseModels;

[ExcludeFromCodeCoverage]
public class CaseResult
{
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
    public Trading Trading { 
        get {
            if (!string.IsNullOrEmpty(tradingNames) && tradingNames != Common.NoTradingNames)
            {
                var serializer = new XmlSerializer(typeof(Trading));

                using (StringReader reader = new StringReader(tradingNames))
                {
                    return (Trading)serializer.Deserialize(reader);
                }
            }
            else 
            {
                return null;
            }
        } 
    }
    
    [NotMapped]
    public IIRRecordType RecordType {
        get 
        {
            switch (insolvencyType) {
                case InsolvencyType.BANKRUPTCY:
                    if (hasRestrictions && restrictionsType == RestrictionsType.ORDER)
                        return IIRRecordType.BRO;
                    else if (hasRestrictions && restrictionsType == RestrictionsType.UNDERTAKING)
                        return IIRRecordType.BRU;
                    else if (hasRestrictions && restrictionsType == RestrictionsType.INTERIMORDER)
                        return IIRRecordType.IBRO;
                    else
                        return IIRRecordType.BKT;
                case InsolvencyType.IVA:
                    return IIRRecordType.IVA;
                case InsolvencyType.DRO:
                    if (hasRestrictions && restrictionsType == RestrictionsType.ORDER)
                        return IIRRecordType.DRRO;
                    else if (hasRestrictions && restrictionsType == RestrictionsType.UNDERTAKING)
                        return IIRRecordType.DRRU;
                    else
                        return IIRRecordType.DRO;
                default:
                    throw new Exception ("Undefined Insolvency Type");
            }       
        }    
    }

    //CaseDetals are output to XML for all record types with exception of
    //BROs, BRUs, IBROs  (possibly DRROs and DRRUs)
    //  where discharge date is greater than 3 months old
    //  and not discharge not suspended
    public bool IncludeCaseDetailsInXML(DateTime now)
    {
        bool result = true;

        switch (RecordType)
        {
            case IIRRecordType.BRO:
            case IIRRecordType.BRU:
            case IIRRecordType.IBRO:
                if (DateOnly.ParseExact(insolvencyDate, "d/M/yyyy", CultureInfo.InvariantCulture).AddMonths(15).ToDateTime(new TimeOnly(0,0,0)) < now
                    && !(caseStatus??"").StartsWith("Discharge Suspended Indefinitely")
                    && !((caseStatus??"").StartsWith("Discharge Fixed Length Suspension")
                            //May work if first term evaluates as true => caseStatus Start with Fixed Length Suspension
                            && DateOnly.ParseExact(caseStatus[54..64], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddMonths(3) < DateOnly.FromDateTime(DateTime.Now))
                    )
                    result = false;
                break;
            default:
                break;
        }

        return result;
    }

    public bool HasIPDetails {
        get 
        { 
            return insolvencyPractitionerName != null && insolvencyPractitionerAddress != null; 
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