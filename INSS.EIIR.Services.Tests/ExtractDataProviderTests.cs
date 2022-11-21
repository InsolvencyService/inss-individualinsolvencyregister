using Azure.Storage.Blobs;
using FluentAssertions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace INSS.EIIR.Services.Tests
{    
    public  class ExtractDataProviderTests
    {
        public ExtractDataProviderTests()
        {
            Environment.SetEnvironmentVariable("blobcontainername", "eiirextractjobs");
            Environment.SetEnvironmentVariable("storageconnectionstring", "UseDevelopmentStorage=true");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1, 4)]
        [InlineData(1, 5)]
        [InlineData(1, 6)]
        public async Task GetExtracts_WithPagination(int pageNumber, int pageSize)
        {
            var logger = new Mock<ILoggerFactory>();
            var dbOptions = new Mock<IOptions<DatabaseConfig>>();
            var blobServiceClient = new Mock<BlobServiceClient>();

            var expectedResult = GetExtracts().Take(pageSize);
            
            var pagingModel = new PagingParameters { PageNumber = pageNumber, PageSize = pageSize };
            var repositoryMock = new Mock<IExtractRepository>();
            repositoryMock
                .Setup(m => m.GetExtractsAsync(pagingModel))
                .ReturnsAsync(expectedResult);

            repositoryMock
                .Setup(m => m.GetTotalExtractsAsync())
                .ReturnsAsync(6);

            var service = new ExtractDataProvider(logger.Object, repositoryMock.Object, dbOptions.Object, blobServiceClient.Object);

            var result = await service.ListExtractsAsync(pagingModel);
            var extracts = result.Extracts.ToList();

            repositoryMock.Verify(m => m.GetExtractsAsync(pagingModel), Times.Once);
            repositoryMock.Verify(m => m.GetTotalExtractsAsync(), Times.Once);

            result.Paging.ResultCount.Should().Be(6);

            extracts.Count.Should().Be(pageSize);
            extracts.First().ExtractId.Should().BeGreaterThan(0);
            extracts.First().DownloadZiplink.Should().Contain("zip");
        }

        [Fact]
        public async Task GetLatestExtract_For_Download()
        {
            
            var logger = new Mock<ILoggerFactory>();
            var dbOptions = new Mock<IOptions<DatabaseConfig>>();
            var blobServiceClient = new Mock<BlobServiceClient>();

            var expectedResult = GetLatestExtract();
            
            var repositoryMock = new Mock<IExtractRepository>();
            repositoryMock
                .Setup(m => m.GetLatestExtractForDownload())
                .ReturnsAsync(expectedResult);


            var service = new ExtractDataProvider(logger.Object, repositoryMock.Object, dbOptions.Object, blobServiceClient.Object);
            var result = await service.GetLatestExtractForDownload();

            repositoryMock.Verify(m => m.GetLatestExtractForDownload(), Times.Once);

            result.Should().Be(expectedResult);
            result.ExtractId.Should().BeGreaterThan(0);
            result.DownloadZiplink.Should().Contain("zip");

        }

        private static Extract GetLatestExtract()
        {
            return new Extract
            {
                ExtractId = 20221021,
                SnapshotCompleted = "Y",
                SnapshotDate = new DateTime(2022, 10, 21),
                ExtractCompleted = "Y",
                ExtractDate = new DateTime(2022, 10, 21),
                ExtractFilename = "3CO5I3H7",
                DownloadZiplink = "http://www.insolvency.gov.uk/eiir/3CO5I3H7.zip",
                DownloadLink = "http://www.insolvency.gov.uk/eiir/3CO5I3H7.xml"
            };
        }

        private static IEnumerable<Extract> GetExtracts()
        {
            return new List<Extract>
            {
                new()
                {
                    ExtractId = 20221021,
                    SnapshotCompleted = "Y",
                    SnapshotDate = new DateTime(2022, 10, 21),
                    ExtractCompleted = "Y",
                    ExtractDate = new DateTime(2022, 10, 21),
                    ExtractFilename = "3CO5I3H7",
                    DownloadZiplink = "http://www.insolvency.gov.uk/eiir/3CO5I3H7.zip",
                    DownloadLink = "http://www.insolvency.gov.uk/eiir/3CO5I3H7.xml"
                },
                new()
                {
                    ExtractId = 20221020,
                    SnapshotCompleted = "Y",
                    SnapshotDate = new DateTime(2022, 10, 20),
                    ExtractCompleted = "Y",
                    ExtractDate = new DateTime(2022, 10, 20),
                    ExtractFilename = "2CO5I3H7",
                    DownloadZiplink = "http://www.insolvency.gov.uk/eiir/2CO5I3H7.zip",
                    DownloadLink = "http://www.insolvency.gov.uk/eiir/2CO5I3H7.xml"

                },
                new()
                {
                    ExtractId = 20221019,
                    SnapshotCompleted = "Y",
                    SnapshotDate = new DateTime(2022, 10, 19),
                    ExtractCompleted = "Y",
                    ExtractDate = new DateTime(2022, 10, 19),
                    ExtractFilename = "1CO5I3H7",
                    DownloadZiplink = "http://www.insolvency.gov.uk/eiir/1CO5I3H7.zip",
                    DownloadLink = "http://www.insolvency.gov.uk/eiir/1CO5I3H7.xml"

                },
                new()
                {
                    ExtractId = 20221018,
                    SnapshotCompleted = "Y",
                    SnapshotDate = new DateTime(2022, 10, 18),
                    ExtractCompleted = "Y",
                    ExtractDate = new DateTime(2022, 10, 18),
                    ExtractFilename = "0CO5I3H7",
                    DownloadZiplink = "http://www.insolvency.gov.uk/eiir/0CO5I3H7.zip",
                    DownloadLink = "http://www.insolvency.gov.uk/eiir/0CO5I3H7.xml"

                },
                new()
                {
                    ExtractId = 20221017,
                    SnapshotCompleted = "Y",
                    SnapshotDate = new DateTime(2022, 10, 17),
                    ExtractCompleted = "Y",
                    ExtractDate = new DateTime(2022, 10, 17),
                    ExtractFilename = "9BO5I3H7",
                    DownloadZiplink = "http://www.insolvency.gov.uk/eiir/9BO5I3H7.zip",
                    DownloadLink = "http://www.insolvency.gov.uk/eiir/9BO5I3H7.xml"

                },
                new()
                {
                    ExtractId = 20221016,
                    SnapshotCompleted = "Y",
                    SnapshotDate = new DateTime(2022, 10, 16),
                    ExtractCompleted = "Y",
                    ExtractDate = new DateTime(2022, 10, 16),
                    ExtractFilename = "8BO5I3H7",
                    DownloadZiplink = "http://www.insolvency.gov.uk/eiir/8BO5I3H7.zip",
                    DownloadLink = "http://www.insolvency.gov.uk/eiir/8BO5I3H7.xml"
                },
            };
        }
    }
}
