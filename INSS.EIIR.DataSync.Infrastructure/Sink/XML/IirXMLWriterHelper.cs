using System.Globalization;
using System.Xml;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using Microsoft.IdentityModel.Abstractions;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public static class IirXMLWriterHelper
    {

        public static void WriteIirReportRequestToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

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
                    WriteIirBktRestrictionDetailsToStream(model, ref xmlStream);
                }

                if (model.RecordType == Models.CaseModels.IIRRecordType.BKT && model.IncludeCaseDetailsInXML)
                {
                    writer.WriteStartElement(null, "CaseDetailsText", null);
                    writer.WriteString($"Insolvency Case Details");
                    writer.WriteEndElement();

                    writer.Flush();
                    WriteIirBktCaseDetailsToStream(model, ref xmlStream);
                }

                writer.WriteEndElement();
                writer.WriteRaw("\r\n");
                writer.Flush();
            }

        }


        public static void WriteIirIndividualDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

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
                writer.WriteString($"{model.individualForenames}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "Surname", null);
                writer.WriteString($"{model.individualSurname}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "Occupation", null);
                writer.WriteString($"{model.individualOccupation}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "DateofBirth", null);
                writer.WriteString($"{model.individualDOB}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "LastKnownAddress", null);
                writer.WriteString($"{model.individualAddress}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "LastKnownPostCode", null);
                writer.WriteString($"{model.individualPostcode}");
                writer.WriteEndElement();

                //OtherNames in getEiirIndex currently come accross as comma separated string e.g. "lastname firstname secondname, lastname firstname"
                //This is a bit flaky especially if someone puts an comma in a name field... and yes it has happened
                //INSSight have been asked to provide other names as XML, they are looking to handroll it as there tools aren't currently configured to hand XML or JSON
                #region othernames
                writer.WriteStartElement(null, "OtherNames", null);

                if (model.individualAlias == Common.NoOtherNames)
                    writer.WriteString($"{model.individualAlias}");
                else 
                {
                    var othernames = model.individualAlias.Split(",", StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries);

                    foreach (var othername in othernames) 
                    {
                        writer.WriteStartElement(null, "OtherName", null);

                        var names = othername.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        //write out forenames, there may not be any
                        for (int i = 1; i < names.Length; i++) {
                            writer.WriteString($"{names[i]} ");
                        }
                        //write out surname for which there will always be one
                        writer.WriteString($"{names[0]}");

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

        public static void WriteIirBktCaseDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(xmlStream, settings))
            {
                writer.WriteStartElement(null, "CaseDetails", null);

                    writer.WriteStartElement(null, "CaseNoCase", null);
                    writer.WriteString($"{model.caseNo}");
                    writer.WriteEndElement();

                    writer.WriteStartElement(null, "CaseName", null);
                    writer.WriteString($"{model.caseName}");
                    writer.WriteEndElement();

                    writer.WriteStartElement(null, "Court", null);
                    writer.WriteString($"{model.courtName}");
                    writer.WriteEndElement();

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
                    writer.WriteString($"{Alter_BKTStatusFormat(model.caseStatus)}");
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
                    writer.WriteString($"{model.caseDescription}");
                    writer.WriteEndElement();

                #region TradingNames
                    writer.WriteStartElement(null, "TradingNames", null);

                    if (model.Trading == null)
                        writer.WriteString($"No Trading Names Found");
                    else 
                    {
                        foreach (var td in model.Trading.TradingDetails)
                        {
                            writer.WriteStartElement(null, "TradingName", null);
                            writer.WriteString($"{td.TradingName}");
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

        //Alters the BKT Status - typically date formats from dd/MM/yyyy to dd MMMM yyyy
        private static string Alter_BKTStatusFormat (string source) 
        {
            if ((source.StartsWith("Discharged On ", StringComparison.OrdinalIgnoreCase) && source.Length ==24 ) ||
                  source.StartsWith("ANNULLED", StringComparison.OrdinalIgnoreCase))
                return source[0..(source.Length - 10)] + DateTime.ParseExact(source[^10..^0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy");

            if (source.StartsWith("Currently Bankrupt : Automatic Discharge", StringComparison.OrdinalIgnoreCase))
                return $"Currently Bankrupt : Automatic Discharge  will be  {DateTime.ParseExact(source[^10..^0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")}";

            //This scenario current uses dd/MM/yyyy format for date element
            //if (source.StartsWith("Discharge Suspended Indefinitely", StringComparison.OrdinalIgnoreCase))
            //    return $"Discharge Suspended Indefinitely (from {DateTime.ParseExact(source[39..49], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")})";

            if (source.StartsWith("Discharge Fixed Length Suspension", StringComparison.OrdinalIgnoreCase))
                return $"Discharge Fixed Length Suspension (from "
                    + $"{DateTime.ParseExact(source[40..50], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")} to "
                    + $"{DateTime.ParseExact(source[54..64], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")})";

            if (source.Contains("(Early Discharge)"))
                return $"Discharged On {DateTime.ParseExact(source[14..24], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMMM yyyy")} (Early Discharge)";

            return source;
        
        }

        public static void WriteIirBktRestrictionDetailsToStream(InsolventIndividualRegisterModel model, ref MemoryStream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

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
                    case IIRRecordType.DRRO:
                        writer.WriteString(RestrictionsTypeXmlText.DRRO);
                        break;
                    case IIRRecordType.DRRU:
                        writer.WriteString(RestrictionsTypeXmlText.DRRU);
                        break;
                    default:
                        throw new Exception($"Unknown Restrictions Type encountered generating XML for {model.caseNo}");
                }
                writer.WriteEndElement();

                writer.WriteStartElement(null, "RestrictionsStartDate", null);
                writer.WriteString($"{model.restrictionsStartDate.Value.ToString("dd/MM/yyyy")}");
                writer.WriteEndElement();

                writer.WriteStartElement(null, "RestrictionsEndDate", null);
                writer.WriteString($"{(model.restrictionsEndDate.HasValue ? model.restrictionsEndDate.Value.ToString("dd/MM/yyyy") : "")}");
                writer.WriteEndElement();

                if (!model.IncludeCaseDetailsInXML) 
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
    }
}
