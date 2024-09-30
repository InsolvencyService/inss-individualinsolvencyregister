using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.AISearch
{
    public static class IndexNameHelper
    {
        public const string EiirIndividuals = "eiir_individuals";

        public static async Task<string> GetNewIndexName(AsyncPageable<string> indexNames)
        {
            var todaysIndexName = $"{AISearchSink.SEARCH_INDEX_BASE_NAME}-{DateTime.Today.ToString("dd-MM-yyyy")}";
            var todaysIndexAttempt = $"{todaysIndexName}-1";

            if (await indexNames.AnyAsync(n => n.StartsWith(todaysIndexName)))
            {
                var todaysLastIndex = await indexNames.Where(i => i.StartsWith(todaysIndexName)).OrderBy(x => x).LastAsync();
                int attemptNumber = Convert.ToInt32(todaysLastIndex.Substring(todaysLastIndex.LastIndexOf('-') + 1));
                todaysIndexAttempt = $"{todaysIndexName}-{attemptNumber + 1}";
            }

            return todaysIndexAttempt;
        }

        public static IEnumerable<string> GetIndexNamesToDelete(IEnumerable<string> names, string keepMe)
        {
            var keepList = new List<string>
            {
                keepMe,
                EiirIndividuals
            };

            var yesterday = DateTime.Today.AddDays(-1);
            var lastDayWanted = DateTime.Today.AddDays(-5);
            var dateRange = GetDateRange(lastDayWanted, yesterday);

            foreach (var dateTime  in dateRange)
            {
                keepList.AddRange(names.Where(x => x.Contains(dateTime.ToString("dd-MM-yyyy"))));
            }

            return names.Except(keepList);
        }

        private static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }
    }
}
