using AutoMapper;
using Azure.Search.Documents.Indexes;
using INSS.EIIR.AzureSearch.Services;
using INSS.EIIR.AzureSearch.Services.QueryServices;
using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions.Functions;
using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.AutoMapperProfiles;
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
        }

        [Fact (Skip = "Dependency on appsettings.json .. which perhaps not available in github")]
        public async Task Feedback_GetAllFeedback_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<CaseDetails>>();

            var caseResult = new CaseResult()
            {
                individualForenames = "Bill",
                individualSurname = "Smith"
            };
            var individualQueryServiceMock = new Mock<IIndividualQueryService>();
            individualQueryServiceMock
                .Setup(m => m.SearchDetailIndexAsync(new CaseRequest()))
                .ReturnsAsync(caseResult);
            var caseDetailsFunc = new CaseDetails(logger, individualQueryServiceMock.Object);

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
