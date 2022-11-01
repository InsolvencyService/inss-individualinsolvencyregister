using FluentAssertions;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;
using Moq;
using Xunit;

namespace INSS.EIIR.Services.Tests
{
    public class CaseDataProviderTests
    {
        [Fact]
        public async void GetIndividualSearchData_Calls_Correct_Repo_Method()
        {
            var expectedResult = GetData();
            var caseRequest = new CaseRequest();

            var repositoryMock = new Mock<ICaseQueryRepository>();
            repositoryMock
                .Setup(m => m.GetCaseAsync(caseRequest))
                .Returns(expectedResult);

            var service = new CaseDataProvider(repositoryMock.Object);

            var result = await service.GetCaseByCaseNoIndivNoAsync(caseRequest); 

            repositoryMock.Verify(m => m.GetCaseAsync(caseRequest), Times.Once);

            result.indvidualForenames.Should().Be("Bill");
            result.indvidualSurname.Should().Be("Smith");
        }

        private static async Task<CaseResult> GetData()
        {
            return new CaseResult()
                {
                    indvidualForenames = "Bill",
                    indvidualSurname = "Smith"
            };
        }
    }
}