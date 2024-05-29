using FluentAssertions;
using INSS.EIIR.Data.Models;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace INSS.EIIR.DataAccess.Tests
{
    public class IndividualRepositorySearchIntegrationTest
    {
        private readonly string _connectionString;

        public IndividualRepositorySearchIntegrationTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = configuration.Build();

            _connectionString = config.GetConnectionString("iirwebdbContextConnectionString");
        }

        [Fact (Skip = "Expensive Integration test ... no appsettings.json to be had")]
        public void SearchByName_Returns_Correct_Number_Of_Rows()
        {
            //Arrange
            var expectedResultsCount = 32;

            var context = new EIIRContext(_connectionString);

            var repository = new IndividualRepository(context);

            //Act
            var results = repository.BuildEiirIndex().ToList();

            //Assert
            results.Count.Should().Be(expectedResultsCount);
        }
    }
}