using Azure;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Tests.IndexNameHelper
{
    public static class GetNewIndexNameTestsData
    {
        public const string INDEX_BASE_NAME = "eiir-individuals";
        public const string DATETIME_TOSTRING = "yyyy-MM-dd";
        public const string NON_PERMITTED_DATA = "ContainsNonPermittedData";

        public static IEnumerable<object[]> GetNewIndexNameData()
        {
            yield return NoNames();
            yield return Yesterdays();
            yield return AlreadyHasTodays();
        }

        public static object[] NoNames()
        {
            var page = Page<string>.FromValues(new List<string>(), continuationToken: null, new Mock<Response>().Object);

            var pages = AsyncPageable<string>.FromPages(new[] { page });

            var expectedValue = $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-1";
            
            return new object[] { pages, expectedValue };
        }

        public static object[] Yesterdays()
        {
            var page = Page<string>.FromValues(new List<string>
            {
               $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-1).ToString(DATETIME_TOSTRING)}-1",
               $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-2).ToString(DATETIME_TOSTRING)}-1",
               $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-3).ToString(DATETIME_TOSTRING)}-1_{NON_PERMITTED_DATA}"
            }, continuationToken: null, new Mock<Response>().Object);

            var pages = AsyncPageable<string>.FromPages(new[] { page });

            var expectedValue = $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-1";

            return new object[] { pages, expectedValue };
        }

        public static object[] AlreadyHasTodays()
        {
            var page = Page<string>.FromValues(new List<string>
            {
               $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-2_{NON_PERMITTED_DATA}",
               $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-1",
               $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-1).ToString(DATETIME_TOSTRING)}-1"
            }, continuationToken: null, new Mock<Response>().Object);

            var pages = AsyncPageable<string>.FromPages(new[] { page });

            var expectedValue = $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-3";

            return new object[] { pages, expectedValue };
        }
    }
}
