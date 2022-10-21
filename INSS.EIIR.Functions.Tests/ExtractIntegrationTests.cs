using AutoMapper;
using Azure.Storage.Blobs;
using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions.Functions;
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
    private readonly ExtractDataProvider _extractDataProvider;
    private readonly IMapper _mapper;

    public ExtractIntegrationTests()
    {
        Environment.SetEnvironmentVariable("blobcontainername", "eiirextractjobs");

        MapperConfiguration mapperConfig = new MapperConfiguration(
         cfg =>
         {
             cfg.AddProfile(new ExtractMapper());
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
        _extractDataProvider = new ExtractDataProvider(logger.Object, _extractRepository, dbOptions.Object, blobServiceClient.Object);

    }

    [Fact]
    public async Task Extract_ListExtracts_Returns_OkResult()
    {
        //Arrange
        var logger = Mock.Of<ILogger<Extract>>();
        var extractFunc = new Extract(logger, _extractDataProvider);

        Mock<HttpRequest> mockRequest = CreateMockRequest();

        //Act
        var response = await extractFunc.ListExtracts(mockRequest.Object) as OkObjectResult;

        //Assert
        Assert.IsType<OkObjectResult>(response);

    }

    private static Mock<HttpRequest> CreateMockRequest()
    {
        var ms = new MemoryStream();
        var sw = new StreamWriter(ms);

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(ms);

        return mockRequest;
    }
}