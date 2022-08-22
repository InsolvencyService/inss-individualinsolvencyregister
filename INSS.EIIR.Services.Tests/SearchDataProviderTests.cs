using FluentAssertions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models;
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
                .Setup(m => m.SearchByName(string.Empty, string.Empty))
                .Returns(expectedResult);

            var service = new SearchDataProvider(repositoryMock.Object);

            var result  = service.GetIndividualSearchData(string.Empty, string.Empty).ToList();

            repositoryMock.Verify(m => m.SearchByName(string.Empty, string.Empty), Times.Once);

            result.Count.Should().Be(1);
            result.First().FirstName.Should().Be("Bill");
            result.First().Surname.Should().Be("Smith");
        }

        private static IEnumerable<SearchResult> GetData()
        {
            return new List<SearchResult>
            {
                new()
                {
                    FirstName = "Bill",
                    Surname = "Smith"
                }
            };
        }
    }
}