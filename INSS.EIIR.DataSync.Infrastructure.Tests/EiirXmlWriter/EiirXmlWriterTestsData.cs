﻿using Azure;
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
            expectedresults.Add(705489723,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>705489723</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>705489723</CaseNoIndividual><Title>Mrs</Title><Gender>Female</Gender><FirstName>ANAS IMOGEN</FirstName><Surname>REID</Surname><Occupation>Unemployed</Occupation><DateofBirth>12/03/1993</DateofBirth><LastKnownAddress>276 Stockport Rd, Gee Cross, Hyde, United Reiddom</LastKnownAddress><LastKnownPostCode>SK14 5RF</LastKnownPostCode><OtherNames><OtherName>GEMMA MORGAN</OtherName><OtherName>GEMMA AGUIRRE</OtherName></OtherNames></IndividualDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>705489723</CaseNoCase><CaseName>Anas Imogen Reid</CaseName><Court>(Court does not apply to DRO)</Court><CaseType>Debt Relief Order</CaseType><CourtNumber>7121348</CourtNumber><CaseYear>2024</CaseYear><StartDate>09/04/2024</StartDate><Status>Debt Relief Order Revoked on 25 June 2024</Status><CaseDescription>Anas Imogen Reid, Unemployed of 276 Stockport Rd, Gee Cross, Hyde, Greater Manchester, SK14 5RF,United Reiddom formerly of 9 Pavilion Gardens, Westhoughton, Bolton BL5 3AJ, United Reiddom</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>705489723</CaseNoContact><InsolvencyServiceOffice>DRO Team</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>1st Floor, Cobourg House, Mayflower Street, PLYMOUTH, United Kingdom</ContactAddress><ContactPostCode>PL1 1DJ</ContactPostCode><ContactTelephone>0300 678 0015</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(70235351,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>70235351</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>70235351</CaseNoIndividual><Title>MR</Title><Gender>Male</Gender><FirstName>WILHELMINA ANNE</FirstName><Surname>AMES</Surname><Occupation>Unemployed</Occupation><DateofBirth>13/04/1964</DateofBirth><LastKnownAddress></LastKnownAddress><LastKnownPostCode>BA22 7ND</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>70235351</CaseNoCase><CaseName>WILHELMINA ANNE AMES</CaseName><Court>County Court at Yeovil</Court><CaseType>Bankruptcy</CaseType><CourtNumber>0000613</CourtNumber><CaseYear>2007</CaseYear><StartDate>06/08/2007</StartDate><Status>Discharge Suspended Indefinitely (from 03/03/2008)</Status><CaseDescription>WILHELMINA ANNE AMES OCCUPATION UNKNOWN OF 18 Grange Road SWINDON SN83 3YT</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>70235351</CaseNoContact><InsolvencyServiceOffice>Exeter</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>Senate Court, Southernhay Gardens, EXETER, United Kingdom</ContactAddress><ContactPostCode>EX1 1UG</ContactPostCode><ContactTelephone>01392 889650</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(365896321,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>365896321</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>365896321</CaseNoIndividual><Title>Mr</Title><Gender>Male</Gender><FirstName>SIDNEY ROBYN</FirstName><Surname>HAYES</Surname><Occupation>Unemployed</Occupation><DateofBirth>18/07/1966</DateofBirth><LastKnownAddress>6 St Fagans St, Caerphilly, Holywell, United Kingdom</LastKnownAddress><LastKnownPostCode>CF83 1FZ</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>365896321</CaseNoCase><CaseName>Sidney Robyn Hayes</CaseName><Court>Office of the Adjudicator</Court><CaseType>Bankruptcy</CaseType><CourtNumber>5088435</CourtNumber><CaseYear>2019</CaseYear><StartDate>16/10/2019</StartDate><Status>Discharged On 17 June 2024</Status><CaseDescription>Sidney Robyn Hayes, Employed, Director, of 6 St Fagans St, Caerphilly, Holywell, Flintshire, CF83 1FZ,   formerly of 138 High St, Billericay CM12 9DF</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>365896321</CaseNoContact><InsolvencyServiceOffice>North West</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>PO Box 16649, BIRMINGHAM, United Kingdom</ContactAddress><ContactPostCode>B2 2PB</ContactPostCode><ContactTelephone>0300 678 0016</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(714256423,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>714256423</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>714256423</CaseNoIndividual><Title>MR</Title><Gender>Male</Gender><FirstName>JEROME WILSON</FirstName><Surname>BLACK</Surname><Occupation>Unemployed</Occupation><DateofBirth>01/04/1953</DateofBirth><LastKnownAddress>51 Belvoir St, LEICESTER, United Kingdom</LastKnownAddress><LastKnownPostCode>LE1 6SL</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><BankruptcyRestrictionsDetails><RestrictionsType>BANKRUPTCY RESTRICTIONS ORDER (BRO)</RestrictionsType><RestrictionsStartDate>16/03/2015</RestrictionsStartDate><RestrictionsEndDate>15/03/2025</RestrictionsEndDate></BankruptcyRestrictionsDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>714256423</CaseNoCase><CaseName>JEROME WILSON BLACK</CaseName><Court>County Court at Leicester</Court><CaseType>Bankruptcy</CaseType><CourtNumber>0000107</CourtNumber><CaseYear>2013</CaseYear><StartDate>29/07/2013</StartDate><Status>Discharge Suspended Indefinitely (from 01/07/2014)</Status><CaseDescription>JEROME WILSON BLACK AKA JR BLACK of 51 Belvoir St, Leicester, LE1    6SL previously at 14 Hotel St,Leicester.</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>714256423</CaseNoContact><InsolvencyServiceOffice>Nottingham</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>PO Box 16651, BIRMINGHAM, United Kingdom</ContactAddress><ContactPostCode>B2 2HQ</ContactPostCode><ContactTelephone>0300 678 0016</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(704598634,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>704598634</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>704598634</CaseNoIndividual><Title>Miss</Title><Gender>Female</Gender><FirstName>TAMZIN MARTHA</FirstName><Surname>OROZCO</Surname><Occupation>Employed</Occupation><DateofBirth>12/10/1990</DateofBirth><LastKnownAddress>6 Northgate, Baildon, Shipley, United Kingdom</LastKnownAddress><LastKnownPostCode>BD17 6JX</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><BankruptcyRestrictionsDetails><RestrictionsType>BANKRUPTCY RESTRICTIONS UNDERTAKING (BRU)</RestrictionsType><RestrictionsStartDate>30/04/2024</RestrictionsStartDate><RestrictionsEndDate>29/04/2034</RestrictionsEndDate><RestrictionsCourt>Office of the Adjudicator</RestrictionsCourt><RestrictionsCourtNo>5148965</RestrictionsCourtNo><RestrictionsCaseYear>2023</RestrictionsCaseYear></BankruptcyRestrictionsDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>704598634</CaseNoContact><InsolvencyServiceOffice>Leeds</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>PO Box 16647, BIRMINGHAM, United Kingdom</ContactAddress><ContactPostCode>B2 2NQ</ContactPostCode><ContactTelephone>0300 678 0016</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(987214365,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>987214365</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>987214365</CaseNoIndividual><Title>Mr</Title><Gender>Male</Gender><FirstName>DIEGO MOHAMMAD</FirstName><Surname>HOFFAN</Surname><Occupation>Unemployed</Occupation><DateofBirth>24/07/1960</DateofBirth><LastKnownAddress>41 Garden Walk, Metrocentre, GATESHEAD, United Kingdom</LastKnownAddress><LastKnownPostCode>NE11 9XZ</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><BankruptcyRestrictionsDetails><RestrictionsType>BANKRUPTCY RESTRICTIONS ORDER (BRO)</RestrictionsType><RestrictionsStartDate>10/12/2018</RestrictionsStartDate><RestrictionsEndDate>10/05/2028</RestrictionsEndDate><RestrictionsCourt>County Court at Newcastle-upon-Tyne</RestrictionsCourt><RestrictionsCourtNo>45</RestrictionsCourtNo><RestrictionsCaseYear>2016</RestrictionsCaseYear><PreviousIBRO><PreviousIBRONote>This BRO was preceded by an Interim Bankruptcy Restrictions Order (IBRO)</PreviousIBRONote><PreviousIBROStartDate>26/09/2017</PreviousIBROStartDate><PreviousIBROEndDate>10/12/2018</PreviousIBROEndDate></PreviousIBRO></BankruptcyRestrictionsDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>987214365</CaseNoContact><InsolvencyServiceOffice>LTADT PPI Team</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>PO Box 16665, BIRMINGHAM, United Kingdom</ContactAddress><ContactPostCode>B2 2JX</ContactPostCode><ContactTelephone>0300 6780015</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(703658924,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>703658924</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>703658924</CaseNoIndividual><Title>Mr</Title><Gender>Male</Gender><FirstName>ANGUS</FirstName><Surname>KNOX</Surname><Occupation>No Occupation Found</Occupation><DateofBirth>23/03/1968</DateofBirth><LastKnownAddress>85 High St, Felling, GATESHEAD, United Kingdom</LastKnownAddress><LastKnownPostCode>NE10 9LU</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><BankruptcyRestrictionsDetails><RestrictionsType>INTERIM BANKRUPTCY RESTRICTIONS ORDER (IBRO)</RestrictionsType><RestrictionsStartDate>03/01/2020</RestrictionsStartDate><RestrictionsEndDate></RestrictionsEndDate></BankruptcyRestrictionsDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>703658924</CaseNoCase><CaseName>Angus Knox</CaseName><Court>County Court at Newcastle-upon-Tyne</Court><CaseType>Bankruptcy</CaseType><CourtNumber>0000123</CourtNumber><CaseYear>2017</CaseYear><StartDate>20/10/2017</StartDate><Status>Discharge Suspended Indefinitely (from 19/09/2018)</Status><CaseDescription>Angus Knox, a Property Landlord, residing at,85 High St, Felling, Whickham Highway, Gateshead, Tyne &amp; Wear, NE10 9LU.</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>703658924</CaseNoContact><InsolvencyServiceOffice>Newcastle</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>PO Box 16646, BIRMINGHAM, United Kingdom</ContactAddress><ContactPostCode>B2 2PW</ContactPostCode><ContactTelephone>0300 678 0016</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(700365842,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>700365842</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>700365842</CaseNoIndividual><Title>Mr</Title><Gender>Male</Gender><FirstName>JAC</FirstName><Surname>LEBLANC</Surname><Occupation>No Occupation Found</Occupation><DateofBirth>04/06/1969</DateofBirth><LastKnownAddress>17 Upper Northgate St, Chester, GBR</LastKnownAddress><LastKnownPostCode>CH1 4EE</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>700365842</CaseNoCase><CaseName>Jac Leblanc</CaseName><Court>Voluntary Liquidation Cases</Court><CaseType>Individual Voluntary Arrangement</CaseType><CourtNumber></CourtNumber><CaseYear>(Case Year does not apply to IVA)</CaseYear><StartDate>17/08/2012</StartDate><Status>Completed On 27 June 2024</Status><CaseDescription>(Case Description does not apply to IVA)</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyPractitionerText>Insolvency Practitioner Contact Details</InsolvencyPractitionerText><IP><CaseNoIP>700365842</CaseNoIP><MainIP>Pablo Synder</MainIP><MainIPFirm>Debt Movement UK Ltd</MainIPFirm><MainIPFirmAddress>3rd Floor, Marsland House, Marsland Road, Sale, United Kingdom</MainIPFirmAddress><MainIPFirmPostCode>M33 3AQ</MainIPFirmPostCode><MainIPFirmTelephone>0330 380 1707</MainIPFirmTelephone></IP><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>700365842</CaseNoContact><InsolvencyServiceOffice>The Insolvency Service</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>Cannon House, 18 Priory Queensway, Birmingham, United Kingdom</ContactAddress><ContactPostCode>B4 6FD</ContactPostCode><ContactTelephone>0303 003 1742</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(700546823,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>700546823</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>700546823</CaseNoIndividual><Title>Mrs</Title><Gender>Female</Gender><FirstName>JEAN VICTORIA</FirstName><Surname>HART</Surname><Occupation>No Occupation Found</Occupation><DateofBirth>25/09/1967</DateofBirth><LastKnownAddress>68 The Kingsway, Swansea, GBR</LastKnownAddress><LastKnownPostCode>SA1 5HW</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>700546823</CaseNoCase><CaseName>Jean Victoria Hart</CaseName><Court>County Court at Swansea</Court><CaseType>Individual Voluntary Arrangement</CaseType><CourtNumber></CourtNumber><CaseYear>(Case Year does not apply to IVA)</CaseYear><StartDate>07/12/2012</StartDate><Status>Completed On 27 June 2024</Status><CaseDescription>(Case Description does not apply to IVA)</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyPractitionerText>Insolvency Practitioner Contact Details</InsolvencyPractitionerText><IP><CaseNoIP>700546823</CaseNoIP><MainIP>Pablo Synder</MainIP><MainIPFirm>Debt Movement UK Ltd</MainIPFirm><MainIPFirmAddress>3rd Floor, Marsland House, Marsland Road, Sale, United Kingdom</MainIPFirmAddress><MainIPFirmPostCode>M33 3AQ</MainIPFirmPostCode><MainIPFirmTelephone>0330 380 1707</MainIPFirmTelephone></IP><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>700546823</CaseNoContact><InsolvencyServiceOffice>The Insolvency Service</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>Cannon House, 18 Priory Queensway, Birmingham, United Kingdom</ContactAddress><ContactPostCode>B4 6FD</ContactPostCode><ContactTelephone>0303 003 1742</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(706589625,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>706589625</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>706589625</CaseNoIndividual><Title>Ms</Title><Gender>Female</Gender><FirstName>HELEN CELESTE</FirstName><Surname>PARKS</Surname><Occupation>Unemployed</Occupation><DateofBirth>09/12/1970</DateofBirth><LastKnownAddress>42 Blossom St, Ancoats, United Kingdom</LastKnownAddress><LastKnownPostCode>M4 6BF</LastKnownPostCode><OtherNames><OtherName>HELEN CELESTE HAYDEN</OtherName></OtherNames></IndividualDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>706589625</CaseNoCase><CaseName>Helen Celeste Parks</CaseName><Court>(Court does not apply to DRO)</Court><CaseType>Debt Relief Order</CaseType><CourtNumber>6900014</CourtNumber><CaseYear>2023</CaseYear><StartDate>06/07/2023</StartDate><Status>Debt Relief Order Revoked on 26 June 2024</Status><CaseDescription>Helen Celeste Parks, Unemployed of 42 Blossom St, Ancoats , Manchester , M4 6BF, United Kingdom</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>706589625</CaseNoContact><InsolvencyServiceOffice>DRO Team</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>1st Floor, Cobourg House, Mayflower Street, PLYMOUTH, United Kingdom</ContactAddress><ContactPostCode>PL1 1DJ</ContactPostCode><ContactTelephone>0300 678 0015</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(704524936,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>704524936</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>704524936</CaseNoIndividual><Title>Miss</Title><Gender>Female</Gender><FirstName>CRYSTAL DAWN</FirstName><Surname>FERNANDEZ</Surname><Occupation>Unemployed</Occupation><DateofBirth>05/09/1988</DateofBirth><LastKnownAddress>42 Bretonside, Plymouth, United Kingdom</LastKnownAddress><LastKnownPostCode>PL4 0AU</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>704524936</CaseNoCase><CaseName>Crystal Dawn Fernandez</CaseName><Court>(Court does not apply to DRO)</Court><CaseType>Debt Relief Order</CaseType><CourtNumber>7181111</CourtNumber><CaseYear>2024</CaseYear><StartDate>11/06/2024</StartDate><Status>Currently Subject To Debt Relief Order : Moratorium Period will end on 11 June 2025</Status><CaseDescription>Crystal Dawn Fernandez, Unemployed of 42 Bretonside, Plymouth, PL4 0AU,United Kingdom formerly of 10 Fitzwilliam St, Broomhall, Sheffield S1 4JB, United Kingdom</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>704524936</CaseNoContact><InsolvencyServiceOffice>DRO Team</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>1st Floor, Cobourg House, Mayflower Street, PLYMOUTH, United Kingdom</ContactAddress><ContactPostCode>PL1 1DJ</ContactPostCode><ContactTelephone>0300 678 0015</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(778745487,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>778745487</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>778745487</CaseNoIndividual><Title>Mr</Title><Gender>Male</Gender><FirstName>LAYTON</FirstName><Surname>CARROLL</Surname><Occupation>Unemployed</Occupation><DateofBirth>30/07/1965</DateofBirth><LastKnownAddress>64 St Mary’s Butts, READING, United Kingdom</LastKnownAddress><LastKnownPostCode>RG1 2LG</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><DebtReliefRestrictionsDetails><DRORestrictionsType>DEBT RELIEF RESTRICTIONS ORDER (DRRO)</DRORestrictionsType><DRORestrictionsStartDate>07/01/2019</DRORestrictionsStartDate><DRORestrictionsEndDate>06/01/2026</DRORestrictionsEndDate></DebtReliefRestrictionsDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>778745487</CaseNoCase><CaseName>Layton Carroll</CaseName><Court>(Court does not apply to DRO)</Court><CaseType>Debt Relief Order</CaseType><CourtNumber>4798839</CourtNumber><CaseYear>2016</CaseYear><StartDate>08/09/2016</StartDate><Status>Currently Subject To Debt Relief Order : Moratorium Period ended on 08 September 2017</Status><CaseDescription>Layton Carroll, Unemployed of 64 St Mary’s Butts, READING, RG1 2LG, United Kingdom formerly of 227 Caversham Rd, Reading , RG1 8BB, United Kingdom</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>778745487</CaseNoContact><InsolvencyServiceOffice>DRO Team</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>1st Floor, Cobourg House, Mayflower Street, PLYMOUTH, United Kingdom</ContactAddress><ContactPostCode>PL1 1DJ</ContactPostCode><ContactTelephone>0300 678 0015</ContactTelephone></InsolvencyContact></ReportRequest>");
            expectedresults.Add(708523692,$"<ReportRequest><ExtractDate>08/08/2024</ExtractDate><CaseNoReportRequest>708523692</CaseNoReportRequest><IndividualDetailsText>Individual Details</IndividualDetailsText><IndividualDetails><CaseNoIndividual>708523692</CaseNoIndividual><Title>Mr</Title><Gender>Male</Gender><FirstName>OLLY FLYNN</FirstName><Surname>CAMERON</Surname><Occupation>Self-Employed</Occupation><DateofBirth>01/03/1961</DateofBirth><LastKnownAddress>58 Outram St, Sutton-in-Ashfield, United Kingdom</LastKnownAddress><LastKnownPostCode>NG17 4FS</LastKnownPostCode><OtherNames>No OtherNames Found</OtherNames></IndividualDetails><DebtReliefRestrictionsDetails><DRORestrictionsType>DEBT RELIEF RESTRICTION UNDERTAKING (DRRU)</DRORestrictionsType><DRORestrictionsStartDate>18/06/2021</DRORestrictionsStartDate><DRORestrictionsEndDate>17/06/2027</DRORestrictionsEndDate></DebtReliefRestrictionsDetails><CaseDetailsText>Insolvency Case Details</CaseDetailsText><CaseDetails><CaseNoCase>708523692</CaseNoCase><CaseName>Olly Flynn Cameron</CaseName><Court>(Court does not apply to DRO)</Court><CaseType>Debt Relief Order</CaseType><CourtNumber>5043221</CourtNumber><CaseYear>2017</CaseYear><StartDate>22/05/2017</StartDate><Status>Currently Subject To Debt Relief Order : Moratorium Period ended on 22 May 2018</Status><CaseDescription>Olly Flynn Cameron, Self-Employed of 58 Outram St, Sutton-in-Ashfield, Cambridgeshire, NG17 4FS, UnitedKingdom formerly of 131 Eastfield Side, Sutton-in-Ashfield NG17 4JW, United Kingdom</CaseDescription><TradingNames>No Trading Names Found</TradingNames></CaseDetails><InsolvencyContactText>Insolvency Service Contact Details</InsolvencyContactText><InsolvencyContact><CaseNoContact>708523692</CaseNoContact><InsolvencyServiceOffice>DRO Team</InsolvencyServiceOffice><Contact>Enquiry Desk</Contact><ContactAddress>1st Floor, Cobourg House, Mayflower Street, PLYMOUTH, United Kingdom</ContactAddress><ContactPostCode>PL1 1DJ</ContactPostCode><ContactTelephone>0300 678 0015</ContactTelephone></InsolvencyContact></ReportRequest>");

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

        public static IEnumerable<object[]> GetEiirBktXmlWriterData()
        {
            return GetEiirXmlWriterData().Where(x => ((InsolventIndividualRegisterModel)x[0]).IncludeCaseDetailsInXML);
        }

        public static IEnumerable<object[]> GetEiirBktRestrictionXmlWriterData()
        {
            return GetEiirXmlWriterData().Where(x => ((InsolventIndividualRegisterModel)x[0]).RecordType == IIRRecordType.BRO
                                                        || ((InsolventIndividualRegisterModel)x[0]).RecordType == IIRRecordType.BRU
                                                        || ((InsolventIndividualRegisterModel)x[0]).RecordType == IIRRecordType.IBRO);
        }



    }
}
