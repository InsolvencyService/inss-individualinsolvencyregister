﻿using AutoMapper;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using INSS.EIIR.AzureSearch.Services;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.AutoMapperProfiles;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace INSS.EIIR.AzureSearch.Tests
{
    public class SearchIndexServiceTests
    {


        [Fact]
        public async Task PopulateIndex_Calls_Correct_Services()
        {
            //Arrange
            var rawData = GetData().ToList();
            var mappedData = GetMappedData();
            var caseData = mappedData.FirstOrDefault();

            var dataProviderMock = new Mock<ISearchDataProvider>();
            dataProviderMock
                .Setup(m => m.GetIndividualSearchData())
                .Returns(rawData);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<IEnumerable<CaseResult>, IEnumerable<IndividualSearch>>(rawData))
                .Returns(mappedData);

            var indexingResultMock = new Mock<Azure.Response<IndexDocumentsResult>>();
            var indexerMock = new Mock<SearchClient>();
            indexerMock
                .Setup(m => m.MergeOrUploadDocumentsAsync(mappedData, null, default(CancellationToken)))
                .ReturnsAsync(indexingResultMock.Object);
            
            var indexClientMock = new Mock<SearchIndexClient>();
            indexClientMock
                .Setup(m => m.GetSearchClient("eiir_individuals"))
                .Returns(indexerMock.Object);
            
            var service = GetService(indexClientMock.Object, mapperMock.Object, dataProviderMock.Object);

            //Act
            await service.PopulateIndexAsync(Mock.Of<ILogger>());

            //Assert
            dataProviderMock.Verify(m => m.GetIndividualSearchData(), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<CaseResult>, IEnumerable<IndividualSearch>>(rawData), Times.Once);
            indexClientMock.Verify(m => m.GetSearchClient("eiir_individuals"), Times.Once);
            indexerMock.Verify(m => m.MergeOrUploadDocumentsAsync(mappedData, null, default(CancellationToken)), Times.Once);

        }

        private IndividualSearchIndexService GetService(
            SearchIndexClient indexClient,
            IMapper mapper,
            ISearchDataProvider searchDataProvider)
        {
            return new IndividualSearchIndexService(indexClient, mapper, searchDataProvider);
        }

        private IEnumerable<CaseResult> GetData()
        {
            return new List<CaseResult>
            {
                new()
                {
                    individualForenames = "Bill",
                    individualSurname = "Smith"
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