using FluentAssertions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;
using Moq;
using Xunit;

namespace INSS.EIIR.Services.Tests
{
    public class SubscriberDataProviderTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1, 4)]
        [InlineData(1, 5)]
        [InlineData(1, 6)]
        public async Task GetAllSubscribers_WithPagination(int pageNumber, int pageSize)
        {
            var expectedResult = GetAllSubscribers();
            var pagingModel = new PagingParameters { PageNumber = pageNumber, PageSize = pageSize };
            var repositoryMock = new Mock<ISubscriberRepository>();
            repositoryMock
                .Setup(m => m.GetSubscribersAsync())
                .ReturnsAsync(expectedResult);

            var service = new SubscriberDataProvider(repositoryMock.Object);

            var result = (await service.GetSubscribersAsync(pagingModel)).Subscribers.ToList();

            repositoryMock.Verify(m => m.GetSubscribersAsync(), Times.Once);

            result.Count.Should().Be(pageSize);
            result.First().SubscriberId.Should().NotBeNullOrEmpty();
            result.First().OrganisationName.Should().Contain("Test Org");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        public async Task GetActiveSubscribers_WithPagination(int pageNumber, int pageSize)
        {
            var expectedResult = GetActiveSubscribers();
            var pagingModel = new PagingParameters { PageNumber = pageNumber, PageSize = pageSize };
            var repositoryMock = new Mock<ISubscriberRepository>();
            repositoryMock
                .Setup(m => m.GetSubscribersAsync())
                .ReturnsAsync(expectedResult);

            var service = new SubscriberDataProvider(repositoryMock.Object);

            var result = (await service.GetActiveSubscribersAsync(pagingModel)).Subscribers.ToList();

            repositoryMock.Verify(m => m.GetSubscribersAsync(), Times.Once);

            result.Count.Should().Be(pageSize);
            result.First().SubscriberId.Should().NotBeNullOrEmpty();
            result.First().OrganisationName.Should().Contain("Test Org");
            result.First().SubscribedFrom.Should().BeBefore(new DateTime(2022,10,18));
            result.First().SubscribedTo.Should().BeAfter(new DateTime(2022,10,18));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        public async Task GetInActiveSubscribers_WithPagination(int pageNumber, int pageSize)
        {
            var expectedResult = GetInActiveSubscribers();
            var pagingModel = new PagingParameters { PageNumber = pageNumber, PageSize = pageSize };
            var repositoryMock = new Mock<ISubscriberRepository>();
            repositoryMock
                .Setup(m => m.GetSubscribersAsync())
                .ReturnsAsync(expectedResult);

            var service = new SubscriberDataProvider(repositoryMock.Object);

            var result = (await service.GetInActiveSubscribersAsync(pagingModel)).Subscribers.ToList();

            repositoryMock.Verify(m => m.GetSubscribersAsync(), Times.Once);

            result.Count.Should().Be(pageSize);
            result.First().SubscriberId.Should().NotBeNullOrEmpty();
            result.First().OrganisationName.Should().Contain("Test Org");
            result.First().SubscribedFrom.Should().BeBefore(new DateTime(2022, 10, 18));
            result.First().SubscribedTo.Should().BeBefore(new DateTime(2022, 10, 18));
        }

        [Fact]
        public async Task Create_Subscriber()
        {
            var data = new CreateUpdateSubscriber();
            var repositoryMock = new Mock<ISubscriberRepository>();
            
            repositoryMock.Setup(m => m.CreateSubscriberAsync(data));

            var service = new SubscriberDataProvider(repositoryMock.Object);

            await service.CreateSubscriberAsync(data);

            repositoryMock.Verify(m => m.CreateSubscriberAsync(data), Times.Once);
        }

        [Fact]
        public async Task Update_Subscriber()
        {
            var subscriberId = "12345";
            var data = new CreateUpdateSubscriber();
            var repositoryMock = new Mock<ISubscriberRepository>();

            repositoryMock.Setup(m => m.UpdateSubscriberAsync(subscriberId, data));

            var service = new SubscriberDataProvider(repositoryMock.Object);

            await service.UpdateSubscriberAsync(subscriberId, data);

            repositoryMock.Verify(m => m.UpdateSubscriberAsync(subscriberId, data), Times.Once);
        }

        private static IEnumerable<Subscriber> GetAllSubscribers()
        {
            return new List<Subscriber>
            {
                new()
                {
                    SubscriberId = "123456789",
                    OrganisationName = "Test Org 1",
                    AccountActive = "Y",
                    SubscribedFrom = new DateTime(2022, 10, 14),
                    SubscribedTo = new DateTime(2032, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "123456789", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                 new()
                {
                    SubscriberId = "234567890",
                    OrganisationName = "Test Org 2",
                    AccountActive = "Y",
                    SubscribedFrom = new DateTime(2022, 10, 14),
                    SubscribedTo = new DateTime(2032, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "234567890", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                new()
                {
                    SubscriberId = "345678901",
                    OrganisationName = "Test Org 3",
                    AccountActive = "Y",
                    SubscribedFrom = new DateTime(2022, 10, 14),
                    SubscribedTo = new DateTime(2032, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "345678901", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                new()
                {
                    SubscriberId = "456789012",
                    OrganisationName = "Test Org 4",
                    AccountActive = "N",
                    SubscribedFrom = new DateTime(2015, 10, 14),
                    SubscribedTo = new DateTime(2020, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "456789012", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                new()
                {
                    SubscriberId = "567890123",
                    OrganisationName = "Test Org 5",
                    AccountActive = "N",
                    SubscribedFrom = new DateTime(2015, 10, 14),
                    SubscribedTo = new DateTime(2020, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "567890123", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                 new()
                {
                    SubscriberId = "678901234",
                    OrganisationName = "Test Org 6",
                    AccountActive = "N",
                    SubscribedFrom = new DateTime(2015, 10, 14),
                    SubscribedTo = new DateTime(2020, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "678901234", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                }

            };
        }

        private static IEnumerable<Subscriber> GetActiveSubscribers()
        {
            return new List<Subscriber>
            {
                new()
                {
                    SubscriberId = "123456789",
                    OrganisationName = "Test Org 1",
                    AccountActive = "Y",                    
                    SubscribedFrom = new DateTime(2022, 10, 14),
                    SubscribedTo = new DateTime(2032, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "123456789", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                 new()
                {
                    SubscriberId = "234567890",
                    OrganisationName = "Test Org 2",
                    AccountActive = "Y",
                    SubscribedFrom = new DateTime(2022, 10, 14),
                    SubscribedTo = new DateTime(2032, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "234567890", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                  new()
                {
                    SubscriberId = "345678901",
                    OrganisationName = "Test Org 3",
                    AccountActive = "Y",
                    SubscribedFrom = new DateTime(2022, 10, 14),
                    SubscribedTo = new DateTime(2032, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "345678901", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                }
            };
        }

        private static IEnumerable<Subscriber> GetInActiveSubscribers()
        {
            return new List<Subscriber>
            {
                new()
                {
                    SubscriberId = "123456789",
                    OrganisationName = "Test Org 1",
                    AccountActive = "Y",
                    SubscribedFrom = new DateTime(2015, 10, 14),
                    SubscribedTo = new DateTime(2020, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "123456789", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                 new()
                {
                    SubscriberId = "234567890",
                    OrganisationName = "Test Org 2",
                    AccountActive = "Y",
                    SubscribedFrom = new DateTime(2015, 10, 14),
                    SubscribedTo = new DateTime(2020, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "234567890", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                },
                  new()
                {
                    SubscriberId = "345678901",
                    OrganisationName = "Test Org 3",
                    AccountActive = "Y",
                    SubscribedFrom = new DateTime(2015, 10, 14),
                    SubscribedTo = new DateTime(2020, 10, 14),
                    EmailContacts = new List<SubscriberEmailContact>
                    {
                        new SubscriberEmailContact() { SubscriberId = "345678901", EmailAddress= "test@insolvency.gov.uk", CreatedOn = new DateTime(2022, 10, 18)}
                    }
                }
            };
        }
    }
}