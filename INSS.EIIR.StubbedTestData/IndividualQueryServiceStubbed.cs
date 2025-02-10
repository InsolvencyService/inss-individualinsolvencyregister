using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SearchModels;
using static Azure.Core.HttpHeader;
using System;
using System.Text.Json;
using AutoMapper;
using INSS.EIIR.Models.Helpers;
using System.ComponentModel.Design;
using System.Linq;
using INSS.EIIR.DataSync.Infrastructure.Fake.Source;
using AutoMapper.Configuration;

namespace INSS.EIIR.StubbedTestData
{
    public class IndividualQueryServiceStubbed : IIndividualQueryService
    {
        private int PageSize = 10;
        private IMapper _mapper;

        public IndividualQueryServiceStubbed(IMapper mapper)
        {
            _mapper = mapper;   
        }

        Task<CaseResult> IIndividualQueryService.GetAsync(IndividualSearch individualSearch)
        {

            List<IndividualSearch> source = new List<IndividualSearch>();

            using (StreamReader r = new StreamReader("searchdata.json"))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<IndividualSearch>>(json);
            }

            var results = source.Where(r => r.Case_Indiv_No.Equals($"{individualSearch.CaseNumber}_{individualSearch.IndividualNumber}"));

            IndividualSearch result = null;

            if (results.Any())
                result = results.First();

            return Task.FromResult(_mapper.Map<IndividualSearch, CaseResult>(result.SetDynamicTestData()));
        }

        Task<CaseResult> IIndividualQueryService.SearchDetailIndexAsync(CaseRequest caseModel)
        {
            throw new NotImplementedException();
        }

        Task<SearchResults> IIndividualQueryService.SearchIndexAsync(IndividualSearchModel searchModel)
        {

            List<IndividualSearch> source = new List<IndividualSearch>();

            using (StreamReader r = new StreamReader("searchdata.json"))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<IndividualSearch>>(json);
            }

            source.ForEach(x => x = x.SetDynamicTestData());

            List<SearchResult> result = new List<SearchResult> { };

            var searchTerms = searchModel.SearchTerm.Base64Decode().ToLower().Split(' ');

            int numberOfRecs = 0;

            if (searchTerms[0] == "*")
                result = source.Select(d => _mapper.Map<IndividualSearch, SearchResult>(d)).ToList();
            else if (searchTerms.Contains("error"))
                throw new Exception("Here you go Rob");
            else if (int.TryParse(searchTerms[0], out numberOfRecs))
                while (result.Count() < numberOfRecs) {
                    result.AddRange(source.Take(numberOfRecs - result.Count()).Select(d => _mapper.Map<IndividualSearch, SearchResult>(d)).ToList());
                }
            else if (searchTerms.Count() == 1)
                result = source.Where(r => r.FirstName.ToLower().StartsWith(searchTerms[0])).Select(d => _mapper.Map<IndividualSearch, SearchResult>(d)).ToList();
            else if (searchTerms.Count() > 1)
                result = source.Where(r => r.FirstName.ToLower().StartsWith(searchTerms[0]) && r.FamilyName.ToLower().StartsWith(searchTerms.Last())).Select(d => _mapper.Map<IndividualSearch, SearchResult>(d)).ToList();

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
