using AutoMapper;
using Azure;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.AzureSearch.Services;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions.Functions;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models.SearchModels;
using INSS.EIIR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using IndividualSearch = INSS.EIIR.Models.IndexModels.IndividualSearch;

namespace INSS.EIIR.Functions.Tests
{
    public class RebuildIndexesIntegrationTests
    {
        private readonly string _connectionString;
        private readonly string _searchServiceUrl;
        private readonly string _adminApiKey;

        public RebuildIndexesIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = configuration.Build();

            _connectionString = config.GetConnectionString("iirwebdbContextConnectionString");

            var settings = config.GetSection("Settings");
            _searchServiceUrl = settings.GetValue<string>("EIIRIndexUrl");
            _adminApiKey = settings.GetValue<string>("EIIRApiKey");
        }

        [Fact]
        public async Task Run_Builds_And_Populates_Index()
        {
            var timerInfo = new TimerInfo(new DailySchedule(), new ScheduleStatus(), false);
            var loggerMock = new Mock<ILogger<RebuildIndexes>>();

            var mappedData = GetMappedData();

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<IEnumerable<SearchResult>, IEnumerable<IndividualSearch>>(It.IsAny<IEnumerable<SearchResult>>()))
                .Returns(mappedData);

            var service = new RebuildIndexes(GetIndexServices(mapperMock.Object), loggerMock.Object);

            await service.Run(CreateMockRequest().Object);
        }

        private IEnumerable<IIndexService> GetIndexServices(IMapper mapper)
        {
            var context = new EIIRContext(_connectionString);
            var repository = new IndividualRepository(context);
            var dataProvider = new SearchDataProvider(repository);

            return new List<IIndexService>
            {
                new IndividualSearchIndexService(CreateSearchServiceClient(), mapper, dataProvider)
            };
        }

        private SearchIndexClient CreateSearchServiceClient()
        {
            var serviceClient = new SearchIndexClient(new Uri(_searchServiceUrl), new AzureKeyCredential(_adminApiKey));
            return serviceClient;
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

        private static Mock<HttpRequest> CreateMockRequest()
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var headers = new Mock<IHeaderDictionary>();

            headers.Setup(x => x["X-Forwarded-For"]).Returns("127.0.0.1");
            headers.Setup(x => x["x-functions-key"]).Returns("mbhyhterkjopeNwshQ8y8jcZ5vCRBWKU8fY1fu-sSFX-AzFu1FZb0w==");

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(h => h.Headers).Returns(headers.Object);
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }
    }
}