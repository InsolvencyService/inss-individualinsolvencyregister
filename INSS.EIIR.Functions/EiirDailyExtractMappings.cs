using INSS.EIIR.Models.ExtractModels;
using System;

namespace INSS.EIIR.Functions
{
    public  static class EiirDailyExtractMappings
    {

        public  static ReportRequest GetReportRequestFromDoc(IndividualSearchResult doc)
        {
            ReportRequest reportRequest = new ReportRequest();
            reportRequest.IndividualDetails = new IndividualDetails();
            reportRequest.IP = new IP();
            reportRequest.CaseDetails = new CaseDetails();
            reportRequest.InsolvencyContact = new InsolvencyContact();
            reportRequest.ExtractDate = DateTime.Now.ToString("MM/dd/yyyy");
            reportRequest.CaseNoReportRequest = doc.CaseNumber;
            reportRequest.IndividualDetailsText = "Individual Details";
            reportRequest.IndividualDetails.CaseNoIndividual = doc.CaseNumber;
            reportRequest.IndividualDetails.Title = "";
            reportRequest.IndividualDetails.Gender = (string)doc.Gender ?? "";
            reportRequest.IndividualDetails.FirstName = doc.FirstName;
            reportRequest.IndividualDetails.Surname = "";
            reportRequest.IndividualDetails.Occupation = (string)doc.Occupation ?? "";
            reportRequest.IndividualDetails.DateofBirth = doc.DateOfBirth;
            reportRequest.IndividualDetails.LastKnownAddress = (string)doc.LastKnownAddress ?? "";
            reportRequest.IndividualDetails.LastKnownPostCode = (string)doc.LastKnownPostcode ?? ""; ;
            reportRequest.IndividualDetails.OtherNames = (string)doc.AlternativeNames ?? "";
            reportRequest.CaseDetailsText = "Insolvency Case Details";
            reportRequest.CaseDetails.CaseNoCase = doc.CaseNumber;
            reportRequest.CaseDetails.CaseName = (string)doc.CaseName ?? "";
            reportRequest.CaseDetails.Court = (string)doc.Court;
            reportRequest.CaseDetails.CaseType = (string)doc.InsolvencyType ?? "";
            reportRequest.CaseDetails.CourtNumber = doc.CourtNumber.ToString();
            reportRequest.CaseDetails.CaseYear = doc.StartDate.ToString("YYYY");
            reportRequest.CaseDetails.StartDate = (string)doc.StartDate.ToString();
            reportRequest.CaseDetails.Status = (string)doc.CaseStatus ?? "";
            reportRequest.CaseDetails.CaseDescription = (string)doc.CaseDescription ?? "";
            reportRequest.CaseDetails.TradingNames = (string)doc.TradingName ?? "";
            reportRequest.InsolvencyPractitionerText = "Insolvency Practitioner Contact Details";
            reportRequest.IP.CaseNoIP = "";
            reportRequest.IP.MainIP = "";
            reportRequest.IP.MainIPFirm = (string)doc.TradingName ?? "";
            reportRequest.IP.MainIPFirmAddress = (string)doc.TradingAddress ?? ""; ;
            reportRequest.IP.MainIPFirmPostCode = (string)doc.TradingPostcode ?? "";
            reportRequest.IP.MainIPFirmTelephone = "";
            reportRequest.InsolvencyContactText = "Insolvency Service Contact Details";
            reportRequest.InsolvencyContact.CaseNoContact = (string)doc.InsolvencyServiceContact ?? "";
            reportRequest.InsolvencyContact.InsolvencyServiceOffice = (string)doc.InsolvencyServiceOffice ?? "";
            reportRequest.InsolvencyContact.Contact = "Enquiry Desk";
            reportRequest.InsolvencyContact.ContactAddress = (string)doc.InsolvencyServiceAddress ?? "";
            reportRequest.InsolvencyContact.ContactPostCode = (string)doc.InsolvencyServicePostcode ?? "";
            reportRequest.InsolvencyContact.ContactTelephone = (string)doc.InsolvencyServiceTelephone ?? "";

            return reportRequest;
        }
    }
}