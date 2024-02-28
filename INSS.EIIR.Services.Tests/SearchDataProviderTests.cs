using FluentAssertions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;
using Moq;
using Xunit;

namespace INSS.EIIR.Services.Tests
{
    public class SearchDataProviderTests
    {
        [Fact]
        public void GetIndividualSearchData_Calls_Correct_Repo_Method()
        {
            var expectedResult = GetData();

            var repositoryMock = new Mock<IIndividualRepository>();
            repositoryMock
                .Setup(m => m.BuildEiirIndex())
                .Returns(expectedResult);

            var service = new SearchDataProvider(repositoryMock.Object);

            var result  = service.GetIndividualSearchData().ToList();

            repositoryMock.Verify(m => m.BuildEiirIndex(), Times.Once);

            result.Count.Should().Be(1);
            result.First().individualForenames.Should().Be("Bill");
            result.First().individualSurname.Should().Be("Smith");
        }

        private static IEnumerable<CaseResult> GetData()
        {
            return new List<CaseResult>
            {
                new()
                {
                    individualForenames = "Bill",
                    individualSurname = "Smith"
                }
            };
        }
    }
}