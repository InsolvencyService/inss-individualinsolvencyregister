using INSS.EIIR.Web.Extensions;

namespace INSS.EIIR.Web.Extensions.Tests
{
    public class DateTimeExtenstionTests
    {

        public static TheoryData< DateTime?, DateTime?, String> Cases =
            new()
                {
                    { new DateTime(2024, 12, 11), new DateTime(2024, 12, 11), "0 days" },
                    { null, null, "" },
                    { new DateTime(2024, 12, 11), null, "" },
                    { null, new DateTime(2024, 12, 11), "" },
                    { new DateTime(2025, 12, 11), new DateTime(2024, 12, 11), "" },
                    { new DateTime(2024, 12, 11), new DateTime(2024, 12, 12), "1 day" },
                    { new DateTime(2024, 12, 11), new DateTime(2024, 12, 13), "2 days" },
                    { new DateTime(2024, 12, 11), new DateTime(2025, 12, 12), "1 year 1 day" },
                    { new DateTime(2024, 12, 11), new DateTime(2025, 12, 13), "1 year 2 days" },
                    { new DateTime(2024, 12, 11), new DateTime(2029, 12, 09), "4 years 364 days" },
                    { new DateTime(2024, 12, 11), new DateTime(2029, 12, 13), "5 years" },
                    { new DateTime(2024, 12, 11), new DateTime(2030, 06, 05), "5 years" },
                    { new DateTime(2024, 12, 11), new DateTime(2030, 06, 15), "6 years" }
                };

        [Theory, MemberData(nameof(Cases))]
        public void Duration(DateTime? self, DateTime? to, string expected)
        {

            //Arrange
            //Act
            var result = self.Duration(to);

            //Assert
            Assert.Equal<string>(expected, result);
        }
    }
}