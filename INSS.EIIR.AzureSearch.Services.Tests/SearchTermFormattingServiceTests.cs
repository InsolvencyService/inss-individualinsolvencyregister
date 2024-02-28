using FluentAssertions;
using Xunit;

namespace INSS.EIIR.AzureSearch.Services.Tests;

public class SearchTermFormattingServiceTests
{
    [Theory]
    [InlineData("*", "/.*/")]
    [InlineData("*Smith", "/.*Smith/")]
    [InlineData("Smith", "Smith")]
    public void FormatSearchTerm(string test, string expected)
    {
        var service = new SearchTermFormattingService();

        var result = service.FormatSearchTerm(test);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData(null)]
    public void FormatSearchTerm_Handles_Null_And_Empty(string test)
    {
        var service = new SearchTermFormattingService();

        var result = service.FormatSearchTerm(test);

        result.Should().Be(string.Empty);
    }
}