using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SearchModels;
using static Azure.Core.HttpHeader;
using System;

namespace INSS.EIIR.StubbedTestData
{
    public class IndividualQueryServiceStubbed : IIndividualQueryService
    {
        private int PageSize = 10;

        Task<CaseResult> IIndividualQueryService.GetAsync(IndividualSearch individualSearch)
        {
            return Task.FromResult(
                    new CaseResult()
                    {
                         individualForenames= "PAUL BERNARD",
                         individualSurname = "MCEVOY",
                         individualDOB = "No Date of Birth Found        ",
                         individualAlias = "No OtherNames Found",
                             individualAddressWithheld = "N",
                              individualAddress = "15 Canon Road, Liverpool, Merseyside, L6 OBN",
                               caseDescription = "PAUL BERNARD McEVOY of and trading at 15 Canon Road,Liverpool, Merseyside L6 OBN as a LABOURER lately of andlately trading at 18 Guilsted Road, Liverpool, MerseysideL11 2SS as a LABOURER.",
                                caseName = "PAUL BERNARD MCEVOY",
                                 caseNo = 15136875,
                                  caseStatus = "Discharge Suspended Indefinitely (from 28/09/1999)",
                                   caseYear = "1999",
                                    insolvencyType = "Bankruptcy"

      //                         "Case_Indiv_No": "15136875_1",
      //"CaseNumber": "15136875",
      //"IndividualNumber": "1",
      //"GlobalSearchField": "15136875 1 Merseyside",
      //"TradeNamesSearchField": "NOT KNOWN NOT KNOWN",
      //"LastNamesSearchField": "MCEVOY",
      //"ForeNamesSearchField": "PAUL BERNARD",
      //   "FirstName": "PAUL BERNARD",
      //"FamilyName": "MCEVOY",
      //"Title": "Mr",
      //"AlternativeNames": "No OtherNames Found",
      //"Gender": "Male",
      //"Occupation": "No Occupation Found",
      //"LastKnownTown": "Merseyside",
      //"LastKnownAddress": "15 Canon Road, Liverpool, Merseyside, L6 OBN",
      //"LastKnownPostcode": "No Last Known PostCode Found",
      //"AddressWithheld": "N",
      //"DateOfBirth": "No Date of Birth Found        ",
      //"CaseName": "PAUL BERNARD MCEVOY",
      //"Court": "County Court at Liverpool",
      //"CourtNumber": "0000240",
      //"CaseYear": "1999",
      //"InsolvencyType": "Bankruptcy",
      //"InsolvencyDate": "27/05/1999",
      //"NotificationDate": "0001-01-01T00:00:00Z",
      //"CaseStatus": "Discharge Suspended Indefinitely (from 28/09/1999)",
      //"CaseDescription": "PAUL BERNARD McEVOY of and trading at 15 Canon Road,Liverpool, Merseyside L6 OBN as a LABOURER lately of andlately trading at 18 Guilsted Road, Liverpool, MerseysideL11 2SS as a LABOURER.",
      //"TradingData": "<Trading><TradingDetails><TradingName>NOT KNOWN</TradingName><TradingAddress>15 Canon Road, Liverpool, Merseyside, L6 OBN</TradingAddress></TradingDetails><TradingDetails><TradingName>NOT KNOWN</TradingName><TradingAddress>18 Guilsted Road, Liverpool, Merseyside, L11 2SS</TradingAddress></TradingDetails></Trading>",
      //"TradingNames": "NOT KNOWN,NOT KNOWN",
      //"HasRestrictions": false,
      //"RestrictionsType": null,
      //"RestrictionsStartDate": null,
      //"RestrictionsEndDate": null,
      //"HasPrevInterimRestrictionsOrder": false,
      //"PrevInterimRestrictionsOrderStartDate": null,
      //"PrevInterimRestrictionsOrderEndDate": null,
      //"PractitionerName": null,
      //"PractitionerFirmName": null,
      //"PractitionerAddress": null,
      //"PractitionerPostcode": null,
      //"PractitionerTelephone": null,
      //"InsolvencyServiceOffice": "Liverpool",
      //"InsolvencyServiceContact": "Enquiry Desk",
      //"InsolvencyServiceAddress": "2nd Floor, Rosebrae Court, Woodside Ferry Approach, BIRKENHEAD, United Kingdom",
      //"InsolvencyServicePostcode": "CH41 6DU",
      //"InsolvencyServiceTelephone": "0151 666 0220",
      //"DateOfPreviousOrder": null,
      //"DeceasedDate": ""
                    }
                );
        }

        Task<CaseResult> IIndividualQueryService.SearchDetailIndexAsync(CaseRequest caseModel)
        {
            throw new NotImplementedException();
        }

        Task<SearchResults> IIndividualQueryService.SearchIndexAsync(IndividualSearchModel searchModel)
        {

            var result = new List<SearchResult> { 
                new SearchResult() { caseNo = "1000", indivNo = "10", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="John Scott", individualSurname="Macdonald", individualTown="Otley", individualPostcode="LS21 3NR"  },
                new SearchResult() { caseNo = "1001", indivNo = "11", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="Peter", individualSurname="Pan", individualTown="", individualPostcode="BD12 4RT"  },
                new SearchResult() { caseNo = "1002", indivNo = "12", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="Eliza", individualSurname="Hook", individualTown="", individualPostcode="LS21 3NR"  },
                new SearchResult() { caseNo = "1003", indivNo = "13", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="Wendy", individualSurname="Darling", individualTown="No Last Known Town Found", individualPostcode="LS18 4HJ"  },
                new SearchResult() { caseNo = "1004", indivNo = "14", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="Donald", individualSurname="Duncj", individualTown="Anaheim", individualPostcode="No Last Known PostCode Found"  },
                new SearchResult() { caseNo = "1005", indivNo = "15", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="John Scott", individualSurname="Macdonald", individualTown="Otley", individualPostcode="LS21 3NR"  },
                new SearchResult() { caseNo = "1006", indivNo = "16", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="John Scott", individualSurname="Macdonald", individualTown="Otley", individualPostcode="LS21 3NR"  },
                new SearchResult() { caseNo = "1007", indivNo = "17", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="John Scott", individualSurname="Macdonald", individualTown="Otley", individualPostcode="LS21 3NR"  },
                new SearchResult() { caseNo = "1008", indivNo = "18", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="John Scott", individualSurname="Macdonald", individualTown="Otley", individualPostcode="LS21 3NR"  },
                new SearchResult() { caseNo = "1009", indivNo = "19", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="John Scott", individualSurname="Macdonald", individualTown="Otley", individualPostcode="LS21 3NR"  },
                new SearchResult() { caseNo = "1010", indivNo = "10", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="John Scott", individualSurname="Macdonald", individualTown="Otley", individualPostcode="LS21 3NR"  },
                new SearchResult() { caseNo = "1011", indivNo = "11", companyName= "<No Trading Names Found>", individualAlias= "No OtherNames Found", individualForenames="John Scott", individualSurname="Macdonald", individualTown="Otley", individualPostcode="LS21 3NR"  }
            };

            var results = new SearchResults
            {

                Results = result.Skip((searchModel.Page - 1) * PageSize).Take(PageSize).ToList(),
                Paging = new PagingModel(result.Count, searchModel.Page)
                {
                    TotalPages = (int)Math.Ceiling((double)((decimal)result.Count / PageSize))
                }
            };

            return Task.FromResult(results);
        }
    }
}
