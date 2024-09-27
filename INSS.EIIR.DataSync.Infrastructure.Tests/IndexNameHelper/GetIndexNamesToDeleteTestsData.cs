using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Tests.IndexNameHelper
{
    public static class GetIndexNamesToDeleteTestsData
    {
        public const string INDEX_BASE_NAME = "eiir-individuals";
        public const string DATETIME_TOSTRING = "dd-MM-yyyy";

        public static IEnumerable<object[]> GetNamesToDeleteData()
        {
            yield return DataList();
        }

        public static object[] DataList()
        {
            List<string> names = new List<string>()
            {
                $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-2",
                $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-3", // latest index..
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-1).ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-1).ToString(DATETIME_TOSTRING)}-2",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-2).ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-3).ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-4).ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-5).ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-6).ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-7).ToString(DATETIME_TOSTRING)}-1"
            };

            List<string> expected = new List<string>()
            {
                $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-2",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-6).ToString(DATETIME_TOSTRING)}-1",
                $"{INDEX_BASE_NAME}-{DateTime.Today.AddDays(-7).ToString(DATETIME_TOSTRING)}-1"
            };

            var keepMe = $"{INDEX_BASE_NAME}-{DateTime.Today.ToString(DATETIME_TOSTRING)}-3";

            return new object[] { names, expected, keepMe };
        }
    }
}
