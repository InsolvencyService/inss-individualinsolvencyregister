using INSS.EIIR.Models.ExtractModels;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace INSS.EIIR.Functions
{
    public  static class EiirDailyExtractSerializer
    {
        public  static ReportDetails SerializeToXml(Azure.Search.Documents.Models.SearchResults<IndividualSearchResult> searchResults)
        {
            XmlSerializer serializer = new(typeof(ReportDetails));
            StringBuilder stringBuilder = new();
            var extractReport = new ReportDetails();
            var count = searchResults.TotalCount;
            if (count > 0)
            {
                //todo GET Totals from Database .
                extractReport.ExtractVolumes = new ExtractVolumes
                {
                    TotalEntries = count.ToString()
                };
            }
            extractReport.Disclaimer = EiirDailyExtractHelpers.GetDisclaimerText();

            foreach (Azure.Search.Documents.Models.SearchResult<IndividualSearchResult> result in searchResults.GetResults())
            {

                IndividualSearchResult doc = result.Document;
                try
                {
                    var date = (string)doc.StartDate.ToString("yyyy");
                    extractReport.ReportRequest.Add(EiirDailyExtractMappings.GetReportRequestFromDoc(doc));
                }
                catch
                {
                    continue;
                }

            }

            StringWriter sw = new(stringBuilder);
            serializer.Serialize(sw, extractReport);
            sw.Close();

            return extractReport;

        }
    }
}