using Azure.Storage.Blobs;
using FluentAssertions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text;
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

        public static IEnumerable<object[]> ByteReplacementTestData()
        {
            //No Match
            yield return new object[] { "<ReportRequest>", 0, $"This is a first test", $"This is a first test", 0, 20, 20 };

            //Start Offset
            yield return new object[] { "<ReportRequest>", 7, $"Request>This is a first test", $"Request>\r\nThis is a first test", 0, 28, 30 };

            //Start Offset - illigeitimate - which doesn't correspond
            yield return new object[] { "<ReportRequest>", 7, $"equest>This is a first test", $"equest>This is a first test", 0, 27, 27 };

            //Null array - preserve offset in event empty array is encountered
            yield return new object[] { "<ReportRequest>", 7, $"", $"", 7, 0, 0 };

            //End offset 
            yield return new object[] { "<ReportRequest>", 0, $"This is a first test<Repo", $"This is a first test<Repo", 5, 25, 25 };

            //Start and end offset 
            yield return new object[] { "<ReportRequest>", 7, $"Request>This is a first test<Repo", $"Request>\r\nThis is a first test<Repo", 5, 33, 35 };

            //Start, middle and end offset 
            yield return new object[] { "<ReportRequest>", 7, $"Request>This is a <ReportRequest> first test<Repo", $"Request>\r\nThis is a <ReportRequest>\r\n first test<Repo", 5, 49, 53 };

            //Continuous 
            yield return new object[] { "<ReportRequest>", 7, $"Request><ReportRequest><ReportRequest><Repo", $"Request>\r\n<ReportRequest>\r\n<ReportRequest>\r\n<Repo", 5, 43, 49 };

            //Middle 
            yield return new object[] { "<ReportRequest>", 0, $"This is a <ReportRequest> first test", $"This is a <ReportRequest>\r\n first test", 0, 36, 38 };

            //Shorties 
            yield return new object[] { "<ReportRequest>", 0, $"a", $"a", 0, 1, 1 };

            yield return new object[] { "<ReportRequest>", 0, $"<", $"<", 1, 1, 1 };

            yield return new object[] { "<ReportRequest>", 0, $"<R", $"<R", 2, 2, 2 };

            //Exact start 
            yield return new object[] { "<ReportRequest>", 0, $"<ReportRequest>This is a first test", $"<ReportRequest>\r\nThis is a first test", 0, 35, 37 };

            //Exact end
            yield return new object[] { "<ReportRequest>", 0, $"This is a first test<ReportRequest>", $"This is a first test<ReportRequest>\r\n", 0, 35, 37 };

        }

        /// <summary>
        /// Tests the insertion of CrLf characters after specified tag in XML
        /// </summary>
        /// <param name="targetText">The XML Element Tag to target</param>
        /// <param name="offset">Then amount of bytes/characters truncated from the target tag at beginning of srcTxt</param>
        /// <param name="srcText">Input string to be modified</param>
        /// <param name="expectedTxt">Expect result after modification</param>
        /// <param name="expectedOffset">The offset to feed into next buffer</param>
        /// <param name="inputByteCount">The number of bytes/characters in srctxt</param>
        /// <param name="expectedByteCount">The number of bytes/characters in output, to be fed in to chained call</param>
        [Theory]
        [MemberData(nameof(ByteReplacementTestData))]
        public void ByteReplacementTest(string targetText, int offset, string srcText, string expectedTxt, int expectedOffset, int inputByteCount, int expectedByteCount)
        {

            //Arrange
            byte[] bytes = Encoding.UTF8.GetBytes(srcText);

            //Act
            var value = bytes.InsertCrLfAfter(Encoding.UTF8.GetBytes(targetText), offset, inputByteCount);

            //Assert 
            Assert.Equal(expectedByteCount, value.Item3);
            Assert.Equal(expectedOffset, value.Item2);
            Assert.Equal(Encoding.UTF8.GetBytes(expectedTxt), value.Item1);

        }

    }
}
