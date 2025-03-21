﻿using System.Globalization;
using System.Text;
using System.Xml;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using INSS.EIIR.Models;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public static class IirXMLWriterHelper
    {

        public static void WriteIirReportRequestToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteStartElement(null, "ReportRequest", null);
                writer.WriteRaw("\r\n");

                writer.WriteStartElement(null, "ExtractDate", null);
                writer.WriteString(DateTime.Now.ToString("dd/MM/yyyy"));
                writer.WriteEndElement();

                writer.WriteStartElement(null, "CaseNoReportRequest", null);
                writer.WriteString($"{model.caseNo}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "IndividualDetailsText", null);
                writer.WriteString($"Individual Details");
                writer.WriteEndElement();

                //Flush writer out to stream, before the next section you want to add
                //Otherwise the stuff you want before will end up after
                writer.Flush();
                WriteIirIndividualDetailsToStream(model, ref xmlStream);

                if (model.hasRestrictions) 
                {
                    if (model.RecordType == IIRRecordType.BRO || model.RecordType == IIRRecordType.BRU || model.RecordType == IIRRecordType.IBRO)
                        WriteIirBktRestrictionDetailsToStream(model, ref xmlStream);
                    else
                        WriteIirDroRestrictionDetailsToStream(model, ref xmlStream);

                }

                if (model.IncludeCaseDetailsInXML(DateTime.Now))
                {
                    writer.WriteStartElement(null, "CaseDetailsText", null);
                    writer.WriteString($"Insolvency Case Details");
                    writer.WriteEndElement();

                    writer.Flush();
                    WriteIirCaseDetailsToStream(model, ref xmlStream);
                }

                if (model.HasIPDetails) 
                {
                    writer.WriteStartElement(null, "InsolvencyPractitionerText", null);
                    writer.WriteString($"Insolvency Practitioner Contact Details");
                    writer.WriteEndElement();

                    writer.Flush();
                    WriteIpDetailsToStream(model, ref xmlStream);

                }

                writer.WriteStartElement(null, "InsolvencyContactText", null);
                writer.WriteString($"Insolvency Service Contact Details");
                writer.WriteEndElement();

                writer.Flush();
                WriteINSSDetailsToStream(model, ref xmlStream);

                writer.WriteEndElement();
                writer.WriteRaw("\r\n");
                writer.Flush();
            }

        }

        private static void WriteINSSDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteStartElement(null, "InsolvencyContact", null);

                writer.WriteStartElement(null, "CaseNoContact", null);
                writer.WriteString($"{model.caseNo}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "InsolvencyServiceOffice", null);
                writer.WriteString($"{model.insolvencyServiceOffice}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "Contact", null);
                writer.WriteString($"{model.insolvencyServiceContact}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "ContactAddress", null);
                writer.WriteString($"{model.insolvencyServiceAddress}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "ContactPostCode", null);
                writer.WriteString($"{model.insolvencyServicePostcode}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "ContactTelephone", null);
                writer.WriteString($"{model.insolvencyServicePhone}");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.Flush();
            }
        }

        private static void WriteIpDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteStartElement(null, "IP", null);

                writer.WriteStartElement(null, "CaseNoIP", null);
                writer.WriteString($"{model.caseNo}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "MainIP", null);
                writer.WriteString($"{model.insolvencyPractitionerName}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "MainIPFirm", null);
                writer.WriteString($"{model.insolvencyPractitionerFirmName}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "MainIPFirmAddress", null);
                writer.WriteString($"{model.insolvencyPractitionerAddress}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "MainIPFirmPostCode", null);
                writer.WriteString($"{model.insolvencyPractitionerPostcode}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "MainIPFirmTelephone", null);
                writer.WriteString($"{model.insolvencyPractitionerTelephone}");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.Flush();
            }
        }

        public static void WriteIirIndividualDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteStartElement(null, "IndividualDetails", null);

                writer.WriteStartElement(null, "CaseNoIndividual", null);
                writer.WriteString($"{model.caseNo}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "Title", null);
                writer.WriteString($"{model.individualTitle}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "Gender", null);
                writer.WriteString($"{model.individualGender}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "FirstName", null);
                if (model.individualForenames == null || model.individualForenames == Common.NoForenames)
                    writer.WriteString($"{model.individualForenames}");
                else
                    writer.WriteString($"{model.individualForenames.ToUpper()}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "Surname", null);
                if (model.individualSurname == null || model.individualSurname == Common.NoSurname)
                    writer.WriteString($"{model.individualSurname}");
                else
                    writer.WriteString($"{model.individualSurname.ToUpper()}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "Occupation", null);
                //Fix minor issue orignally fixed in APP-4951 but still in gettEiirIndex
                switch (model.individualOccupation)
                {
                    case "Non Surrender":
                    case "Non Trading":
                    case "Self Employed":
                        writer.WriteString($"{model.individualOccupation.Replace(" ", "-")}");
                        break;
                    default:
                        writer.WriteString($"{model.individualOccupation}");
                        break;
                }              
                writer.WriteEndElement();

                writer.WriteStartElement(null, "DateofBirth", null);
                writer.WriteString($"{model.individualDOB.Trim()}");
                writer.WriteEndElement();

                if (!string.IsNullOrEmpty(model.deceasedDate))
                {
                    writer.WriteStartElement(null, "DeceasedDate", null);
                    writer.WriteString($"{model.deceasedDate}");
                    writer.WriteEndElement();
                }

                writer.WriteStartElement(null, "LastKnownAddress", null);
                writer.WriteString($"{model.individualAddress}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "LastKnownPostCode", null);
                
                if (string.IsNullOrWhiteSpace(model.individualPostcode))
                    writer.WriteString(Common.NoLastKnownPostCode);
                else
                    writer.WriteString($"{model.individualPostcode}");
                writer.WriteEndElement();

                //OtherNames in getEiirIndex currently come accross as comma separated string e.g. "lastname firstname secondname, lastname firstname"
                //This is a bit flaky especially if someone puts an comma in a name field... and yes it has happened
                //INSSight have been asked to provide other names as XML, they are looking to handroll it as there tools aren't currently configured to hand XML or JSON
                #region othernames
                writer.WriteStartElement(null, "OtherNames", null);

                if (model.individualAlias == Common.NoOtherNames)
                    writer.WriteString($"{model.individualAlias}");
                //APP-5394 Othernames not currently output for IVAs
                else if (model.RecordType == IIRRecordType.IVA)
                    writer.WriteString(Common.NoOtherNames);
                else 
                {
                    foreach (var othername in model.OtherNames.Names)
                    {
                        writer.WriteStartElement(null, "OtherName", null);
                        writer.WriteString($"{(othername.Forenames ?? "").ToUpper()} {(othername.Surname ?? "").ToUpper()}".Trim());
                        writer.WriteEndElement();
                    }               
                }

                writer.WriteEndElement();
                #endregion othernames

                writer.WriteEndElement();
                writer.WriteRaw("\r\n");
                writer.Flush();
            }

        }

        public static void WriteIirCaseDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteStartElement(null, "CaseDetails", null);

                writer.WriteStartElement(null, "CaseNoCase", null);
                writer.WriteString($"{model.caseNo}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "CaseName", null);
                if (model.caseName == null)
                    writer.WriteString($"{model.caseName}");
                else
                    writer.WriteString($"{model.caseName}");
                writer.WriteEndElement();

                //APP-5725 Donot output Court for DRRO for consitency with earlier behaviour
                if (model.RecordType != IIRRecordType.DRRO)
                { 
                    writer.WriteStartElement(null, "Court", null);               
                    writer.WriteString($"{model.courtName}");
                    writer.WriteEndElement();
                }

                writer.WriteStartElement(null, "CaseType", null);
                writer.WriteString($"{model.insolvencyType}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "CourtNumber", null);
                writer.WriteString($"{model.courtNumber}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "CaseYear", null);
                writer.WriteString($"{model.caseYear}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "StartDate", null);
                writer.WriteString($"{model.insolvencyDate}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "Status", null);

                switch (model.RecordType) 
                {
                    case IIRRecordType.BKT:
                    case IIRRecordType.BRO:
                    case IIRRecordType.BRU:
                    case IIRRecordType.IBRO:
                        writer.WriteString($"{Alter_BKTStatusFormat(model.caseStatus)}");
                        break;
                    case IIRRecordType.IVA:
                        writer.WriteString($"{Alter_IVAStatusFormat(model.caseStatus)}");
                        break;
                    case IIRRecordType.DRO:
                    case IIRRecordType.DRRO:
                    case IIRRecordType.DRRU:
                        writer.WriteString($"{Alter_DROStatusFormat(model.insolvencyDate, model.caseStatus)}");
                        break;
                    default:
                        throw new Exception($"Unable to detemine recordtype for XML Extract for record: {model.caseNo}");
                }
                    
                writer.WriteEndElement();

                if (model.annulDate != null)
                {
                    writer.WriteStartElement(null, "AnnulDate", null);
                    writer.WriteString($"{model.annulDate}");
                    writer.WriteEndElement();

                    writer.WriteStartElement(null, "AnnulReason", null);
                    writer.WriteString($"{model.annulReason}");
                    writer.WriteEndElement();
                }

                writer.WriteStartElement(null, "CaseDescription", null);
                if (model.caseDescription == null)
                    writer.WriteString(Common.NoCaseDescription);
                else
                    writer.WriteString($"{model.caseDescription}");
                writer.WriteEndElement();

                if (!string.IsNullOrEmpty(model.deceasedDate))
                {
                    writer.WriteStartElement(null, "SpecialNote", null);
                    writer.WriteString($"Please note that this person is deceased (Deceased Date {model.deceasedDate})");
                    writer.WriteEndElement();
                }

                #region TradingNames
                writer.WriteStartElement(null, "TradingNames", null);

                    if (model.Trading == null)
                        writer.WriteString($"No Trading Names Found");
                    else 
                    {
                        foreach (var td in model.Trading.TradingDetails)
                        {
                            writer.WriteStartElement(null, "TradingName", null);
                            writer.WriteString($"{td.TradingName.Trim().ToUpper()}");
                            writer.WriteEndElement();

                            if (td != null) { 
                                writer.WriteStartElement(null, "TradingAddress", null);
                                writer.WriteString($"{td.TradingAddress}");
                                writer.WriteEndElement();
                            }
                        }
                    }
                    writer.WriteEndElement();
                #endregion TradingNames

                writer.WriteEndElement();
                writer.Flush();
            }

        }


        //The following transformations are based on the document EIIR Data Properties a copy of which can be
        //found on APP-5332
        private static object Alter_DROStatusFormat(string insolvencyDate, string source)
        {
            var endDate = DateTime.ParseExact(source[^10..^0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var returnValue = source[0..(source.Length - 10)] + DateTime.ParseExact(source[^10..^0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy");

            DateTime dateResult;

            //Following section only applies to approximately 10 of 40000 records
            if (source.Contains("Moratorium Period"))
            {
                if (DateTime.TryParseExact(insolvencyDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dateResult)) 
                {
                    var insolvencyDatePlus12 = dateResult.AddMonths(12);
                    if ((endDate - insolvencyDatePlus12).Days > 1)
                    {
                        return returnValue
                            + $" (Extended From {insolvencyDatePlus12.ToString("dd/MM/yyyy")}"
                             + $" To {endDate.ToString("dd/MM/yyyy")})";
                    }
                }
            }

            return returnValue;
        }

        //Alters the IVA Status - typically date formats from dd/MM/yyyy to dd MMMM yyyy
        private static object Alter_IVAStatusFormat(string source)
        {
            if (source != "Current")
                return source[0..(source.Length - 10)] + DateTime.ParseExact(source[^10..^0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy");

            return source;
        }

        //Alters the BKT Status - typically date formats from dd/MM/yyyy to dd MMMM yyyy
        private static string Alter_BKTStatusFormat (string source) 
        {
            if ((source.StartsWith("Discharged On ", StringComparison.OrdinalIgnoreCase) && source.Length ==24 ) ||
                  source.StartsWith("ANNULLED", StringComparison.OrdinalIgnoreCase))
                return source[0..(source.Length - 10)] + DateTime.ParseExact(source[^10..^0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy");

            if (source.StartsWith("Currently Bankrupt : Automatic Discharge", StringComparison.OrdinalIgnoreCase))
                return $"Currently Bankrupt : Automatic Discharge  will be  {DateTime.ParseExact(source[^10..^0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")}";

            //This scenario current uses dd/MM/yyyy format for date element - commented code left in on purpose should alternative formatting be requried
            //if (source.StartsWith("Discharge Suspended Indefinitely", StringComparison.OrdinalIgnoreCase))
            //    return $"Discharge Suspended Indefinitely (from {DateTime.ParseExact(source[39..49], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")})";

            //This scenario current uses dd/MM/yyyy format for date element - commented code left in on purpose should alternative formatting be requried
            //if (source.StartsWith("Discharge Fixed Length Suspension", StringComparison.OrdinalIgnoreCase))
            //    return $"Discharge Fixed Length Suspension (from "
            //        + $"{DateTime.ParseExact(source[40..50], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")} to "
            //        + $"{DateTime.ParseExact(source[54..64], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")})";

            if (source.Contains("(Early Discharge)"))
                return $"Discharged On {DateTime.ParseExact(source[14..24], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")} (Early Discharge)";

            return source;
        
        }

        public static void WriteIirBktRestrictionDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteStartElement(null, "BankruptcyRestrictionsDetails", null);

                writer.WriteStartElement(null, "RestrictionsType", null);
                switch (model.RecordType)
                {
                    case IIRRecordType.BRO:
                        writer.WriteString(RestrictionsTypeXmlText.BRO);
                        break;
                    case IIRRecordType.BRU:
                        writer.WriteString(RestrictionsTypeXmlText.BRU);
                        break;
                    case IIRRecordType.IBRO:
                        writer.WriteString(RestrictionsTypeXmlText.IBRO);
                        break;
                    default:
                        throw new Exception($"Unknown Restrictions Type encountered in WriteIirBktRestrictionDetailsToStream generating XML for {model.caseNo}");
                }
                writer.WriteEndElement();

                writer.WriteStartElement(null, "RestrictionsStartDate", null);
                writer.WriteString($"{model.restrictionsStartDate.Value.ToString("dd/MM/yyyy")}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "RestrictionsEndDate", null);
                writer.WriteString($"{(model.restrictionsEndDate.HasValue ? model.restrictionsEndDate.Value.ToString("dd/MM/yyyy") : "")}");
                writer.WriteEndElement();

                if (!model.IncludeCaseDetailsInXML(DateTime.Now)) 
                {
                    writer.WriteStartElement(null, "RestrictionsCourt", null);
                    writer.WriteString(model.courtName);
                    writer.WriteEndElement();

                    writer.WriteStartElement(null, "RestrictionsCourtNo", null);
                    writer.WriteString(model.courtNumber.TrimStart('0'));
                    writer.WriteEndElement();

                    writer.WriteStartElement(null, "RestrictionsCaseYear", null);
                    writer.WriteString(model.caseYear);
                    writer.WriteEndElement();
                }

                if (model.hasaPrevInterimRestrictionsOrder) 
                {
                    writer.WriteStartElement(null, "PreviousIBRO", null);

                    writer.WriteStartElement(null, "PreviousIBRONote", null);
                    writer.WriteString(RestrictionsTypeXmlText.PREVIBRONOTE);
                    writer.WriteEndElement();

                    writer.WriteStartElement(null, "PreviousIBROStartDate", null);
                    writer.WriteString($"{model.prevInterimRestrictionsOrderStartDate.Value.ToString("dd/MM/yyyy")}");
                    writer.WriteEndElement();

                    writer.WriteStartElement(null, "PreviousIBROEndDate", null);
                    writer.WriteString($"{model.prevInterimRestrictionsOrderEndDate.Value.ToString("dd/MM/yyyy")}");
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();

            }
        }

        public static void WriteIirDroRestrictionDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteStartElement(null, "DebtReliefRestrictionsDetails", null);

                writer.WriteStartElement(null, "DRORestrictionsType", null);
                switch (model.RecordType)
                {
                    case IIRRecordType.DRRO:
                        writer.WriteString(RestrictionsTypeXmlText.DRRO);
                        break;
                    case IIRRecordType.DRRU:
                        writer.WriteString(RestrictionsTypeXmlText.DRRU);
                        break;
                    default:
                        throw new Exception($"Unknown Restrictions Type encountered in WriteIirDroRestrictionDetailsToStream generating XML for {model.caseNo}");
                }
                writer.WriteEndElement();

                writer.WriteStartElement(null, "DRORestrictionsStartDate", null);
                writer.WriteString($"{model.restrictionsStartDate.Value.ToString("dd/MM/yyyy")}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "DRORestrictionsEndDate", null);
                writer.WriteString($"{(model.restrictionsEndDate.HasValue ? model.restrictionsEndDate.Value.ToString("dd/MM/yyyy") : "")}");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.Flush();

            }
        }

        public static void WriteIirHeaderToStream(ref MemoryStream? xmlStream, ExtractVolumes ev)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteRaw("<?xml version='1.0' encoding='utf-8'?><ReportDetails>");

                writer.WriteStartElement(null, "ExtractVolumes", null );

                writer.WriteStartElement(null, "TotalEntries", null);
                writer.WriteString($"{ev.TotalEntries}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "TotalBanks", null);
                writer.WriteString($"{ev.TotalBanks}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "TotalIVAs", null);
                writer.WriteString($"{ev.TotalIVAs}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "NewBanks", null);
                writer.WriteString($"{ev.NewBanks}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "TotalDros", null);
                writer.WriteString($"{ev.TotalDros}");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteStartElement(null, "Disclaimer", null);
                writer.WriteString($"While every effort has been made to ensure that the information provided is accurate, occasionally errors may occur. If you identify information which appears to be incorrect or omitted, please inform The Insolvency Service so that we can investigate the matter and correct the database as required.The Insolvency Case Details are taken from the Court Order made on the Order Date, and include the address(es) from which debts were incurred.They cannot be changed without the consent of the Court. The Individual Details may have changed since the Court Order but, even so, they might not reflect the person's current address or occupation at the time you make your search, and they should not be relied on as such. The Insolvency Service cannot accept responsibility for any errors or omissions as a result of negligence or otherwise. Please note that The Insolvency Service and Official Receivers cannot provide legal or financial advice. You should seek this from a Citizen's Advice Bureau, a solicitor, a qualified accountant, an authorised Insolvency Practitioner, reputable financial advisor or advice centre. The Individual Insolvency Register is a publicly available register and The Insolvency Service does not endorse, nor make any representations regarding, any use made of the data on the register by third parties.");
                writer.WriteEndElement();

                writer.Flush();
            }
        }
        public static void WriteIirFooterToStream(ref MemoryStream? xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            settings.ConformanceLevel = ConformanceLevel.Fragment;

            

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteRaw("</ReportDetails>\r\n");
                writer.Flush();
            }
        }
    }
}
