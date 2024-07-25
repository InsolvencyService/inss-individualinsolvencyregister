using AutoMapper;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.AutoMapperProfiles;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using FluentAssertions;
using INSS.EIIR.AzureSearch.Services.Constants;
using INSS.EIIR.AzureSearch.Services.ODataFilters;
using INSS.EIIR.AzureSearch.Services.QueryServices;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.SearchModels;
using Moq;
using Xunit;
using INSS.EIIR.Models.Helpers;

namespace INSS.EIIR.AzureSearch.Services.Tests;

public class IndividualQueryServiceIntegrationTests
{
    [Fact]
    public async Task ApplyFilter()
    {
        var courtFilter = "(Court eq '1' or Court eq '2')";
        var courtNameFilter = "(CourtName eq 'foo' or CourtName eq 'foo2')";

        //APP-5144 Search Term is now base64 encoded in front end
        //It is decoded within the BaseQueryService and passed to formatting and cleaning services unencoded
        var searchTerm = ("Bob").Base64Encode();

        var expected = new SearchResult
        {
            caseNo = "12345"
        };


        var indexModel = new IndividualSearch() { CaseNumber = "12345" };

        var searchModel = new IndividualSearchModel
        {
            CourtNames = new List<string>{ "foo", "foo2" },
            Courts = new List<string>{ "1", "2" },
            SearchTerm = searchTerm
        };

        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(m => m.Map<IndividualSearch, SearchResult>(It.IsAny<IndividualSearch>()))
            .Returns(expected);

        var responseMock = new Mock<Response>();

        var searchClientMock = new Mock<SearchClient>();
        searchClientMock
            .Setup(m => m.SearchAsync<IndividualSearch>(searchTerm.Base64Decode(), It.IsAny<SearchOptions>(), It.IsAny<CancellationToken>()))
            .Returns(
                Task.FromResult(
                    Response.FromValue(
                        SearchModelFactory.SearchResults(new[]
                            {
                                SearchModelFactory.SearchResult(indexModel, 0.9, null)
                            },
                            100,
                            null,
                            null,
                            responseMock.Object),
                        responseMock.Object)));

        var indexClientMock = new Mock<SearchIndexClient>();
        indexClientMock.Setup(m => m.GetSearchClient(SearchIndexes.EiirIndividuals)).Returns(searchClientMock.Object);

        
        var formattingServiceMock = new Mock<ISearchTermFormattingService>();
        formattingServiceMock.Setup(m => m.FormatSearchTerm(searchTerm.Base64Decode())).Returns(searchTerm.Base64Decode());

        var cleaningServiceMock = new Mock<ISearchCleaningService>();
        cleaningServiceMock.Setup(m => m.EscapeFilterSpecialCharacters(courtFilter)).Returns(courtFilter);
        cleaningServiceMock.Setup(m => m.EscapeFilterSpecialCharacters(courtNameFilter)).Returns(courtNameFilter);
        cleaningServiceMock.Setup(m => m.EscapeSearchSpecialCharacters(searchTerm.Base64Decode())).Returns(searchTerm.Base64Decode());

        var service = new IndividualQueryService(mapperMock.Object, indexClientMock.Object, formattingServiceMock.Object, cleaningServiceMock.Object, GetFilters(cleaningServiceMock.Object));

        var result  = (await service.SearchIndexAsync(searchModel));

        result.Results.Count.Should().Be(1);
        result.Results.First().caseNo.Should().Be("12345");
    }

    private IEnumerable<IIndiviualSearchFilter> GetFilters(ISearchCleaningService cleaningSer4viceMock)
    {
        return new List<IIndiviualSearchFilter>
        {
            new IndividualSearchCourtFilter(cleaningSer4viceMock),
            new IndividualSearchCourtNameFilter(cleaningSer4viceMock)
            
        };
    }
}