using AutoMapper;
using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions.Functions;
using INSS.EIIR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace INSS.EIIR.Functions.Tests
{
    public class SubscriberIntegrationTests
    {
        private readonly string _connectionString;
        private readonly EIIRContext _context;
        private readonly SubscriberRepository _subscriberRepository;
        private readonly SubscriberDataProvider _subscriberDataProvider;
        private readonly IMapper _mapper;

        public SubscriberIntegrationTests()
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(
             cfg =>
             {
                 cfg.AddProfile(new SubscriberMapper());
             });

            _mapper = new Mapper(mapperConfig);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = configuration.Build();

            _connectionString = config.GetConnectionString("iirwebdbContextConnectionString");
            _context = new EIIRContext(_connectionString);
            _subscriberRepository = new SubscriberRepository(_context, _mapper);
            _subscriberDataProvider = new SubscriberDataProvider(_subscriberRepository);

        }

        [Fact]
        public async Task Subscriber_GetAllSubscribers_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();
            var subscriberFunc = new Subscriber(logger, _subscriberDataProvider);

            Mock<HttpRequest> mockRequest = CreateMockRequest();

            //Act
            var response = await subscriberFunc.GetSubscribers(mockRequest.Object) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);

        }

        [Fact]
        public async Task Subscriber_GetActiveSubscribers_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();            
            var subscriberFunc = new Subscriber(logger, _subscriberDataProvider);

            Mock<HttpRequest> mockRequest = CreateMockRequest();

            //Act
            var response = await subscriberFunc.GetActiveSubscribers(mockRequest.Object) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);

        }

        [Fact]
        public async Task Subscriber_GetSubscriberById_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();
            var subscriberFunc = new Subscriber(logger, _subscriberDataProvider);

            Mock<HttpRequest> mockRequest = CreateMockGetWithParamRequest("12345");

            //Act
            var response = await subscriberFunc.GetSubscriberById(mockRequest.Object) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task Subscriber_GetInactiveSubscribers_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();
            var subscriberFunc = new Subscriber(logger, _subscriberDataProvider);

            Mock<HttpRequest> mockRequest = CreateMockRequest();

            //Act
            var response = await subscriberFunc.GetInactiveSubscribers(mockRequest.Object) as OkObjectResult;

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

        private static Mock<HttpRequest> CreateMockGetWithParamRequest(string queryParam)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(ms);

            var paramsDictionary = new Dictionary<string, StringValues>
            {
                { "id", queryParam }
            };

            mockRequest.Setup(i => i.Query).Returns(new QueryCollection(paramsDictionary));
    
            return mockRequest;
        }      
    }
}
