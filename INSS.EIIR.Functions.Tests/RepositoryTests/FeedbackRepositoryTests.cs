using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

using INSS.EIIR.Data.AutoMapperProfiles;
using INSS.EIIR.Interfaces.DataAccess;

namespace INSS.EIIR.Functions.Tests.RepositoryTests
{
    public class FeedbackRepositoryTests
    {
        private IMapper _mapper;

        [Fact]
        public void HardDeleteTest()
        {
            var options = new DbContextOptionsBuilder<EIIRContext>()
                .UseInMemoryDatabase(databaseName: "EIIRFeedbackDatabase")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new EIIRContext(options))
            {
                context.CiCaseFeedback.Add(new CiCaseFeedback { FeedbackId = 1, Viewed = false, FeedbackDate = new DateTime(2022, 09, 30, 14, 25, 10), CaseName = "", InsolvencyType="", Message="", ReporterEmailAddress="", ReporterFullname="", ReporterOrganisation="" });
                context.CiCaseFeedback.Add(new CiCaseFeedback { FeedbackId = 2, Viewed = true, ViewedDate = new DateTime(2022, 06, 01, 14, 25, 10), FeedbackDate = new DateTime(2022, 05, 30, 14, 25, 10), CaseName = "", InsolvencyType = "", Message = "", ReporterEmailAddress = "", ReporterFullname = "", ReporterOrganisation = "" });
                context.CiCaseFeedback.Add(new CiCaseFeedback { FeedbackId = 3, Viewed = true, ViewedDate = new DateTime(2022, 08, 01, 14, 25, 10), FeedbackDate = new DateTime(2022, 07, 30, 14, 25, 10), CaseName = "", InsolvencyType = "", Message = "", ReporterEmailAddress = "", ReporterFullname = "", ReporterOrganisation = "" });
                context.SaveChanges();
            }

            MapperConfiguration mapperConfig = new(
                 cfg =>
                 {
                     cfg.AddProfile(new FeedbackMapper());
                 });

            _mapper = new Mapper(mapperConfig);

            var systemDateTimeMock = new Mock<TimeProvider>();
            systemDateTimeMock
                .Setup(m => m.GetUtcNow())
                .Returns(new DateTimeOffset(new DateTime(2022, 10, 30, 14, 25, 10)));


            // Use a clean instance of the context to run the test
            using (var context = new EIIRContext(options))
            {
                IFeedbackRepository feedbackRepository = new FeedbackRepository(context, _mapper, systemDateTimeMock.Object);
                var recordsDeleted = feedbackRepository.DeleteViewedRecords(4);

                Assert.Equal(1, recordsDeleted);
                Assert.Equal(2, context.CiCaseFeedback.Count());
                Assert.Equal(1, context.CiCaseFeedback.Where(x => x.Viewed == false).ToList().First().FeedbackId);
                Assert.Equal(3, context.CiCaseFeedback.Where(x => x.Viewed == true).ToList().First().FeedbackId);

            }
        }

    }
}
