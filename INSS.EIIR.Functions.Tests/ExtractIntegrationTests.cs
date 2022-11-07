using AutoMapper;
using Azure.Storage.Blobs;
using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions.Functions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.AutoMapperProfiles;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace INSS.EIIR.Functions.Tests;

public class ExtractIntegrationTests
{

    private readonly string _connectionString;
    private readonly EIIRExtractContext _eiirExtractcontext;
    private readonly EIIRContext _context;
    private readonly ExtractRepository _extractRepository;
    private readonly SubscriberRepository _subscriberRepository;
    private readonly ExtractDataProvider _extractDataProvider;
    private readonly IMapper _mapper;

    public ExtractIntegrationTests()
    {
        Environment.SetEnvironmentVariable("blobcontainername", "eiirdailyextracts");
        Environment.SetEnvironmentVariable("storageconnectionstring", "UseDevelopmentStorage=true");

        MapperConfiguration mapperConfig = new MapperConfiguration(
         cfg =>
         {
             cfg.AddProfile(new ExtractMapper());
             cfg.AddProfile(new SubscriberMapper());
         });

        _mapper = new Mapper(mapperConfig);
        var logger = new Mock<ILoggerFactory>();
        var dbOptions = new Mock<IOptions<DatabaseConfig>>();
        var blobServiceClient = new Mock<BlobServiceClient>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var config = configuration.Build();

        _connectionString = config.GetConnectionString("iirwebdbContextConnectionString");
        _context = new EIIRContext(_connectionString);
        _eiirExtractcontext = new EIIRExtractContext();
        _extractRepository = new ExtractRepository(_eiirExtractcontext, _context, dbOptions.Object, _mapper);
        _subscriberRepository = new SubscriberRepository(_context, _mapper);
        _extractDataProvider = new ExtractDataProvider(logger.Object, _extractRepository, dbOptions.Object, blobServiceClient.Object);        
    }

    [Fact]
    public async Task Extract_ListExtracts_Returns_OkResult()
    {
        //Arrange
        var logger = Mock.Of<ILogger<Extract>>();
        var subscriberDataProvider = new SubscriberDataProvider(_subscriberRepository);
        var extractFunc = new Extract(logger, subscriberDataProvider, _extractDataProvider);
        Mock<HttpRequest> mockRequest = CreateMockRequest();
       
        //Act
        var response = await extractFunc.ListExtracts(mockRequest.Object) as OkObjectResult;

        //Assert
        Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public async Task Extract_GetLatestExtractForDownload_Returns_OkResult()
    {
        //Arrange
        var logger = Mock.Of<ILogger<Extract>>();        
        
        var expectedResult = new Models.SubscriberModels.Subscriber() { SubscriberId = "12345", AccountActive = "Y", SubscribedFrom = DateTime.Today.AddDays(-10), SubscribedTo = DateTime.Today.AddDays(10) };
               
        var repositoryMock = new Mock<ISubscriberRepository>();
        repositoryMock
            .Setup(m => m.GetSubscriberByIdAsync("12345"))
            .ReturnsAsync(expectedResult);

        var extractRepositoryMock = new Mock<IExtractRepository>();
        extractRepositoryMock
            .Setup(m => m.GetLatestExtractForDownload())
            .ReturnsAsync(GetLatestExtract());

        var extractDataProvider = new Mock<IExtractDataProvider>();      
        extractDataProvider.Setup(x => x.DownloadLatestExtractAsync("blobFile"))
                            .ReturnsAsync(new byte[7000]);

        var latestExtract = GetLatestExtract();
        extractDataProvider.Setup(x => x.GetLatestExtractForDownload())
            .ReturnsAsync(latestExtract);

        var subscriberDataProvider = new SubscriberDataProvider(repositoryMock.Object);

        var extractFunc = new Extract(logger, subscriberDataProvider, extractDataProvider.Object);

        Mock<HttpRequest> mockRequest = CreateMockRequest();

        //Act
        var response = await extractFunc.LatestExtract(mockRequest.Object, "12345");

        //Assert
        Assert.IsType<FileContentResult>(response);
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

    private static Mock<HttpRequest> CreateMockGetWithParamRequest(Dictionary<string, StringValues> paramsDictionary)
    {
        var ms = new MemoryStream();
        var sw = new StreamWriter(ms);
        var headers = new Mock<IHeaderDictionary>();
        
        headers.Setup(x => x["X-Forwarded-For"]).Returns("127.0.0.1");
        headers.Setup(x => x["x-functions-key"]).Returns("mbhyhterkjopeNwshQ8y8jcZ5vCRBWKU8fY1fu-sSFX-AzFu1FZb0w==");

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(h => h.Headers).Returns(headers.Object);
        mockRequest.Setup(i => i.Query).Returns(new QueryCollection(paramsDictionary));
        mockRequest.Setup(x => x.Body).Returns(ms);

        return mockRequest;
    }

    private static Models.ExtractModels.Extract GetLatestExtract()
    {
        return new Models.ExtractModels.Extract
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
}