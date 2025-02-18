﻿using AutoMapper;
using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using INSS.EIIR.Functions.Functions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Services;

using Microsoft.Azure.Functions.Worker.Http;

using Microsoft.AspNetCore.Http;
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

        [Fact(Skip = "Expensive integration test, does not mock external objects, dependency on appsettings.json .. which perhaps not available in github")]
        public async Task Subscriber_GetAllSubscribers_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();
            var subscriberFunc = new Subscriber(logger, _subscriberDataProvider);

            Mock<HttpRequestData> mockRequest = CreateMockRequest();

            //Act
            var response = await subscriberFunc.GetSubscribers(mockRequest.Object) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);

        }
       
        [Fact(Skip = "Expensive integration test, does not mock external objects, dependency on appsettings.json .. which perhaps not available in github")]
        public async Task Subscriber_GetActiveSubscribers_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();            
            var subscriberFunc = new Subscriber(logger, _subscriberDataProvider);

            Mock<HttpRequestData> mockRequest = CreateMockRequest();

            //Act
            var response = await subscriberFunc.GetActiveSubscribers(mockRequest.Object) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);

        }

        [Fact(Skip = "Expensive integration test, does not mock external objects, dependency on appsettings.json .. which perhaps not available in github")]
        public async Task Subscriber_GetSubscriberById_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();
            var expectedResult = new Models.SubscriberModels.Subscriber() { SubscriberId = "12345", AccountActive = "Y", SubscribedFrom = DateTime.Today.AddDays(-10), SubscribedTo = DateTime.Today.AddDays(10) };

            var repositoryMock = new Mock<ISubscriberRepository>();
            repositoryMock
                .Setup(m => m.GetSubscriberByIdAsync("12345"))
                .ReturnsAsync(expectedResult);

            var subscriberDataProvider = new SubscriberDataProvider(repositoryMock.Object);
            var subscriberFunc = new Subscriber(logger, subscriberDataProvider);

            Mock<HttpRequestData> mockRequest = CreateMockRequest();

            //Act
            var response = await subscriberFunc.GetSubscriberById(mockRequest.Object, "12345") as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact(Skip = "Expensive integration test, does not mock external objects, dependency on appsettings.json .. which perhaps not available in github")]
        public async Task Subscriber_GetSubscriberById_Returns_NotFoundResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();
            var subscriberFunc = new Subscriber(logger, _subscriberDataProvider);

            Mock<HttpRequestData> mockRequest = CreateMockRequest();

            //Act
            var response = await subscriberFunc.GetSubscriberById(mockRequest.Object, "12345") as NotFoundObjectResult;

            //Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact(Skip = "Expensive integration test, does not mock external objects, dependency on appsettings.json .. which perhaps not available in github")]
        public async Task Subscriber_GetInactiveSubscribers_Returns_OkResult()
        {
            //Arrange
            var logger = Mock.Of<ILogger<Subscriber>>();
            var subscriberFunc = new Subscriber(logger, _subscriberDataProvider);

            Mock<HttpRequestData> mockRequest = CreateMockRequest();

            //Act
            var response = await subscriberFunc.GetInactiveSubscribers(mockRequest.Object) as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(response);

        }

        private static Mock<HttpRequestData> CreateMockRequest()
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var mockRequest = new Mock<HttpRequestData>();
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }

        private static Mock<HttpRequest> CreateMockGetWithParamRequest(Dictionary<string, StringValues> paramsDictionary)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var headers = new Mock<IHeaderDictionary>();

            headers.Setup(x => x["x-functions-key"]).Returns("mbhyhterkjopeNwshQ8y8jcZ5vCRBWKU8fY1fu-sSFX-AzFu1FZb0w==");

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(h => h.Headers).Returns(headers.Object);
            mockRequest.Setup(i => i.Query).Returns(new QueryCollection(paramsDictionary));
            mockRequest.Setup(x => x.Body).Returns(ms);
                
            return mockRequest;
        }
    }
}
