using Azure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using INSS.EIIR.Models.AutoMapperProfiles;
using INSS.EIIR.Models.IndexModels;

namespace INSS.EIIR.DataSync.Infrastructure.Tests.EiirXmlWriter
{
    public static class EiirXmlWriterTestsData
    {
        public static IEnumerable<object[]> GetEiirXmlWriterData()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new IndividualSearchMapper());
                mc.AddProfile(new INSS.EIIR.DataSync.Infrastructure.Source.SQL.Models.AutoMapperProfiles.InsolventIndividualRegisterModelMapper());
                mc.AddProfile(new INSS.EIIR.DataSync.Application.UseCase.SyncData.AutoMapperProfiles.InsolventIndividualRegisterModelMapper());
            });

            var mapper = new Mapper(mapperConfig);

            //Ok I know I'm a bad booy for using .Result, but how is one to do it otherwise
            //Perhaps excusable in tests
            return ResultData(mapper).ToListAsync().Result;
        }

        public async static IAsyncEnumerable<object[]> ResultData(Mapper mapper)
        {

            var values = new List<object[]>();

            var expectedresults = new Dictionary<int, string> (){ };
            expectedresults.Add(123589635,$"<ReportRequest><ExtractDate>01/10/2024</ExtractDate><CaseNoReportRequest>123589635</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>123589635</CaseNoIndividual><Title>Mr</Title><Gender>Male</Gender><FirstName>JULIAN CARL</FirstName><Surname>GRANT-RIDDLE</Surname><Occupation>No Occupation Found</Occupation><DateofBirth>12/06/1960</DateofBirth><LastKnownAddress>4 Castle St, Conwy, United Kingdom</LastKnownAddress><LastKnownPostCode>LL32 8AY</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>123589635</CaseNoCase><CaseName>Julian Carl Grant-Riddle</CaseName><Court>County Court at Carmarthen</Court><CaseType>Bankruptcy</CaseType><CourtNumber>0000076</CourtNumber><CaseYear>2024</CaseYear><StartDate>22/05/2024</StartDate><Status>Currently Bankrupt : Automatic Discharge  will be  22 May 2025</Status><CaseDescription>Julian Carl Grant-Riddle trading as ABC Buildersresiding and carrying on business at 4 Castle St Conwy LL32 8AY</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyPractitionerText>Insolvency Practitioner Contact Details</InsolvencyPractitionerText><IP><CaseNoIP>123589635</CaseNoIP><MainIP>Carl Freeman</MainIP><MainIPFirm>PKF Littlejohn Advisory</MainIPFirm><MainIPFirmAddress>15 Westferry Circus, LONDON, United Kingdom</MainIPFirmAddress><MainIPFirmPostCode>E14 4HD</MainIPFirmPostCode><MainIPFirmTelephone>020 7516 2200</MainIPFirmTelephone></IP><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>123589635</CaseNoContact><InsolvencyServiceOffice>Cardiff</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>PO Box 16655, BIRMINGHAM, United Kingdom</ContactAddress><ContactPostCode>B2 2EP</ContactPostCode><ContactTelephone>0300 678 0016</ContactTelephone></InsolvencyContact></ReportRequest>");

            using (Stream r = new FileStream("..\\..\\..\\..\\INSS.EIIR.StubbedTestData\\searchdata.json", FileMode.Open))
            {
                await foreach (var record in JsonSerializer.DeserializeAsyncEnumerable<IndividualSearch>(r))
                {
                    var iirModel = mapper.Map<CaseResult, InsolventIndividualRegisterModel>(mapper.Map<IndividualSearch, CaseResult>(record));

                    string expectResult;
                    if (expectedresults.TryGetValue(iirModel.caseNo, out expectResult))
                        yield return new object[] {iirModel, expectResult};
                }
            }
        }
    }
}
