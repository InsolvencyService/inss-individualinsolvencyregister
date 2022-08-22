using AutoMapper;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using INSS.EIIR.AzureSearch.IndexModels;
using INSS.EIIR.AzureSearch.Services;
using INSS.EIIR.Interfaces.SearchIndexer;
using INSS.EIIR.Models;
using Moq;
using Xunit;

namespace INSS.EIIR.AzureSearch.Tests
{
    public class SearchIndexServiceTests
    {
        [Fact]
        public void PopulateIndex()
        {
            //Arrange
            var rawData = GetData().ToList();
            var mappedData = GetMappedData();

            var dataProviderMock = new Mock<ISearchDataProvider>();
            dataProviderMock
                .Setup(m => m.GetIndividualSearchData(string.Empty, string.Empty))
                .Returns(rawData);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<IEnumerable<SearchResult>, IEnumerable<IndividualSearch>>(rawData))
                .Returns(mappedData);

            var indexingResultMock = new Mock<Azure.Response<IndexDocumentsResult>>();
            var indexerMock = new Mock<SearchClient>();
            indexerMock
                .Setup(m => m.IndexDocuments(It.IsAny<IndexDocumentsBatch<IEnumerable<IndividualSearch>>>(), null, CancellationToken.None))
                .Returns(indexingResultMock.Object);
            
            var indexClientMock = new Mock<SearchIndexClient>();
            indexClientMock
                .Setup(m => m.GetSearchClient("IndividualSearch"))
                .Returns(indexerMock.Object);
            
            var service = GetService(indexClientMock.Object, mapperMock.Object, dataProviderMock.Object);

            //Act
            service.PopulateIndex();

            //Assert
            dataProviderMock.Verify(m => m.GetIndividualSearchData(string.Empty, string.Empty), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<SearchResult>, IEnumerable<IndividualSearch>>(rawData), Times.Once);

            indexClientMock.Verify(m => m.GetSearchClient("IndividualSearch"), Times.Once);
            indexerMock.Verify(m => m.IndexDocuments(It.IsAny<IndexDocumentsBatch<IEnumerable<IndividualSearch>>>(), null, CancellationToken.None), Times.Once);

        }

        private SearchIndexService GetService(
            SearchIndexClient indexClient,
            IMapper mapper,
            ISearchDataProvider searchDataProvider)
        {
            return new SearchIndexService(indexClient, mapper, searchDataProvider);
        }

        private IEnumerable<SearchResult> GetData()
        {
            return new List<SearchResult>
            {
                new()
                {
                    FirstName = "Bill",
                    Surname = "Smith"
                }
            };
        }

        private IEnumerable<IndividualSearch> GetMappedData()
        {
            return new List<IndividualSearch>
            {
                new()
                {
                    CaseNumber = "1",
                    FirstName = "Bill",
                    FamilyName = "Smith"
                }
            };
        }
    }
}