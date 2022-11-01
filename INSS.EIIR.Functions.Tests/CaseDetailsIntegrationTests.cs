using AutoMapper;
using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions.Functions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace INSS.EIIR.Functions.Tests
{
    public class CaseDetailsIntegrationTests
    {
        private readonly string _connectionString;
        private readonly EIIRContext _context;
        private readonly ICaseQueryRepository _caseQueryRepository;
        private readonly ICaseDataProvider _caseDataProvider;
        private readonly IMapper _mapper;

        public CaseDetailsIntegrationTests()
        {
            MapperConfiguration mapperConfig = new(
             cfg =>
             {
                 cfg.AddProfile(new FeedbackMapper());
             });


            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = configuration.Build();

            _connectionString = config.GetConnectionString("iirwebdbContextConnectionString");
            _context = new EIIRContext(_connectionString);
            _caseQueryRepository = new CaseQueryRepository(_context);
            _caseDataProvider = new CaseDataProvider(_caseQueryRepository);
        }

        [Fact]
        public async Task Feedback_GetAllFeedback_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<CaseDetails>>();
            var caseDetailsFunc = new CaseDetails(logger, _caseDataProvider);
            var caseResult = new CaseResult()
            {
                indvidualForenames = "Bill",
                indvidualSurname = "Smith"
            };

            Mock<HttpRequest> mockRequest = CreateMockRequest(caseResult);

            //Act
            var response = await caseDetailsFunc.GetCaseDetails(mockRequest.Object) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        private static Mock<HttpRequest> CreateMockRequest(CaseResult caseResult)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var headers = new Mock<IHeaderDictionary>();

            var json = JsonConvert.SerializeObject(caseResult);
            ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

            headers.Setup(x => x["X-Forwarded-For"]).Returns("127.0.0.1");
            headers.Setup(x => x["x-functions-key"]).Returns("mbhyhterkjopeNwshQ8y8jcZ5vCRBWKU8fY1fu-sSFX-AzFu1FZb0w==");

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(h => h.Headers).Returns(headers.Object);
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }
    }
}
