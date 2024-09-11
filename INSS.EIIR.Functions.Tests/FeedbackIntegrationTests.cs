using AutoMapper;
using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions.Functions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace INSS.EIIR.Functions.Tests
{
    public class FeedbackIntegrationTests
    {
        private readonly string _connectionString;
        private readonly EIIRContext _context;
        private readonly FeedbackRepository _feedbackRepository;
        private readonly FeedbackDataProvider _feedbackDataProvider;
        private readonly IMapper _mapper;

        public FeedbackIntegrationTests()
        {
            MapperConfiguration mapperConfig = new(
             cfg =>
             {
                 cfg.AddProfile(new FeedbackMapper());
             });

            _mapper = new Mapper(mapperConfig);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = configuration.Build();

            _connectionString = config.GetConnectionString("iirwebdbContextConnectionString");
            _context = new EIIRContext(_connectionString);
            _feedbackRepository = new FeedbackRepository(_context, _mapper);
            _feedbackDataProvider = new FeedbackDataProvider(_feedbackRepository);
        }

        [Fact (Skip = "Expensive integration test, dependency on appsettings.json .. which perhaps not available in github")]
        public async Task Feedback_GetAllFeedback_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Feedback>>();
            var feedbackFunc = new Feedback(logger, _feedbackDataProvider);
            var feedbackBody = new FeedbackBody()
            {
                PagingModel = new PagingParameters { PageNumber = 1, PageSize = 10 },
                Filters = new FeedbackFilterModel { Status = "All" }
            };

            Mock<HttpRequestData> mockRequest = CreateMockRequest(feedbackBody);

            //Act
            var response = await feedbackFunc.GetFeedback(mockRequest.Object) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        private static Mock<HttpRequestData> CreateMockRequest(FeedbackBody feedbackBody)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var headers = new Mock<HttpHeadersCollection>();

            var json = JsonConvert.SerializeObject(feedbackBody);
            ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

            //headers.Setup(x => x["X-Forwarded-For")).Returns("127.0.0.1");
            //headers.Setup(x => x["x-functions-key"]).Returns("mbhyhterkjopeNwshQ8y8jcZ5vCRBWKU8fY1fu-sSFX-AzFu1FZb0w==");

            var mockRequest = new Mock<HttpRequestData>();
            mockRequest.Setup(h => h.Headers).Returns(headers.Object);
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }
    }
}
