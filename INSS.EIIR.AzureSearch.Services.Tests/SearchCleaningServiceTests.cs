using FluentAssertions;
using Xunit;

namespace INSS.EIIR.AzureSearch.Services.Tests
{
    public class SearchCleaningServiceTests
    {
        [Fact]
        public void EscapeFilterSpecialCharacters()
        {
            const string test = "field = '{0}'";
            const string expected = "field = ''{0}''";

            var service = new SearchCleaningService();

            var result = service.EscapeFilterSpecialCharacters(test);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void EscapeFilterSpecialCharacters_Handles_Null_And_Empty(string test)
        {
            var service = new SearchCleaningService();

            var result = service.EscapeFilterSpecialCharacters(test);

            result.Should().Be(string.Empty);
        }

        [Fact]
        public void EscapeSearchSpecialCharacters()
        {
            var specialCharacters = @"+-&|!(){}[]^""~?:;/`<>#%@=\";
            var escapedCharacters = @"\+\-\&\|\!\(\)\{\}\[\]\^\""\~\?\:\;\/\`\<\>\#\%\@\=\\";

            var test = "Tom Smith" + specialCharacters;
            var expected = "Tom Smith" + escapedCharacters;

            var service = new SearchCleaningService();

            var result = service.EscapeSearchSpecialCharacters(test);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void EscapeSearchSpecialCharacters_Handles_Null_And_Empty(string test)
        {
            var service = new SearchCleaningService();

            var result = service.EscapeSearchSpecialCharacters(test);

            result.Should().Be(string.Empty);
        }
    }
}