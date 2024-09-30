using Azure;
using Moq;

namespace INSS.EIIR.DataSync.Infrastructure.Tests.IndexNameHelper
{
    public class IndexNameHelperTests
    {
        [Theory]
        [MemberData(nameof(GetNewIndexNameTestsData.GetNewIndexNameData), MemberType = typeof(GetNewIndexNameTestsData))]
        public async Task IndexNameHelper_GetNewIndexName_returns_correct_name(AsyncPageable<string> pages, string expected)
        {
            // arrange

            // act
            var name = await Infrastructure.Sink.AISearch.IndexNameHelper.GetNewIndexName(pages);

            // assert
            Assert.Equal(expected, name);
        }

        [Theory]
        [MemberData(nameof(GetIndexNamesToDeleteTestsData.GetNamesToDeleteData), MemberType = typeof(GetIndexNamesToDeleteTestsData))]
        public void IndexNameHelper_GetIndexNamesToDelete_returns_correct_list(IEnumerable<string> names, IEnumerable<string> expected, string keepMe)
        {
            // arrange

            // act
            var list = Infrastructure.Sink.AISearch.IndexNameHelper.GetIndexNamesToDelete(names, keepMe);

            // assert
            Assert.Equal(expected, list);
        }
    }
}