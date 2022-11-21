using FluentAssertions;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.FeedbackModels;
using INSS.EIIR.Models.SubscriberModels;
using Moq;
using Xunit;


namespace INSS.EIIR.Services.Tests
{
    public class FeedbackDataProviderTests
    {
        [Theory]
        [InlineData("All", null, null)]
        [InlineData("Viewed", null, null)]
        [InlineData("Unviewed", null, null)]
        [InlineData("All", "Member of the public", null)]
        [InlineData("Viewed", "Financial services", null)]
        [InlineData("Unviewed", "Government department", null)]
        [InlineData("All", "Member of the public", "I")]
        [InlineData("Viewed", "Financial services", "B")]
        [InlineData("Unviewed", "Government department", "D")]
        public async Task GetFeedback_WithFilters(string viewedStatus, string organisation, string insolvencyType)
        {
            var expectedResult = GetCaseFeedback();
            var feedbackBody = new FeedbackBody() { 
                PagingModel = new PagingParameters { PageNumber = 1, PageSize = 10 }, 
                Filters = new FeedbackFilterModel { Status = viewedStatus, Organisation = organisation, InsolvencyType = insolvencyType } 
            };
            var repositoryMock = new Mock<IFeedbackRepository>();
            repositoryMock
                .Setup(m => m.GetFeedbackAsync())
                .ReturnsAsync(expectedResult);

            var service = new FeedbackDataProvider(repositoryMock.Object);

            var result = (await service.GetFeedbackAsync(feedbackBody)).Feedback.ToList();

            repositoryMock.Verify(m => m.GetFeedbackAsync(), Times.Once);

            result.Count.Should().Be(result.Count);
        }

        [Fact]
        public void Create_Feedback()
        {
            var data = new CreateCaseFeedback();
            var repositoryMock = new Mock<IFeedbackRepository>();

            repositoryMock.Setup(m => m.CreateFeedback(data));

            var service = new FeedbackDataProvider(repositoryMock.Object);

            service.CreateFeedback(data);

            repositoryMock.Verify(m => m.CreateFeedback(data), Times.Once);
        }

        [Fact]
        public void Update_Feedback()
        {
            var feedbackId = 12345;
            var viewedStatus = true;
            var expectedResult = new CaseFeedback() { CaseId = 12345, FeedbackDate = DateTime.Now };
            var repositoryMock = new Mock<IFeedbackRepository>();
            var contextMock = new Mock<EIIRContext>();
            contextMock.Setup(x => x.Add(expectedResult));

            repositoryMock.Setup(m => m.UpdateFeedbackStatus(feedbackId, viewedStatus)).Returns(true);

            var service = new FeedbackDataProvider(repositoryMock.Object);

            var result = service.UpdateFeedbackStatus(feedbackId, viewedStatus);

            repositoryMock.Verify(m => m.UpdateFeedbackStatus(feedbackId, viewedStatus), Times.Once);
            result.Should().BeTrue();   
        }

        private static IEnumerable<CaseFeedback> GetCaseFeedback()
        {
            return new List<CaseFeedback>() {
                new() { 
                    FeedbackDate = new DateTime(2022, 10, 10), 
                    Message = "This is a new feedback message testing create", 
                    ReporterFullname = "Test Feedback", 
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk", 
                    ReporterOrganisation = "Member of the public", 
                    CaseId = 1,
                    InsolvencyType = "B",
                    Viewed = true,
                    ViewedDate = new DateTime(2022, 10, 20)
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 12),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Financial services",
                    CaseId = 2,
                    InsolvencyType = "B",
                    Viewed = true,
                    ViewedDate = new DateTime(2022, 10, 22)
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 14),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Government department",
                    CaseId = 3,
                    InsolvencyType = "I",
                    Viewed = true,
                    ViewedDate = new DateTime(2022, 10, 24)
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 23),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Member of the public",
                    CaseId = 4,
                    InsolvencyType = "D",
                    Viewed = false
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 24),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Financial services",
                    CaseId = 5,
                    InsolvencyType = "B",
                    Viewed = false,
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 25),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Financial services",
                    CaseId = 6,
                    InsolvencyType = "I",
                    Viewed = false,
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 26),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Member of the public",
                    CaseId = 7,
                    InsolvencyType = "I",
                    Viewed = false,
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 26),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Government department",
                    CaseId = 8,
                    InsolvencyType = "D",
                    Viewed = false,    
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 27),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Member of the public",
                    CaseId = 9,
                    InsolvencyType = "B",
                    Viewed = false,                
                },
                new() {
                    FeedbackDate = new DateTime(2022, 10, 27),
                    Message = "This is a new feedback message testing create",
                    ReporterFullname = "Test Feedback",
                    ReporterEmailAddress = "testfeedback@insolvenxy.gov.uk",
                    ReporterOrganisation = "Government department",
                    CaseId = 10,
                    InsolvencyType = "I",
                    Viewed = false,
                },

            };
        }
    }
}
