using Azure;
using Azure.Data.Tables;
using System.Collections.Frozen;

namespace INSS.EIIR.AzureSearch.IndexMapper
{
    public class IndexMapperService : IGetIndexMapService, ISetIndexMapService
    {
        public const string INDEX_MAP_TABLE_NAME = "IndexMap";
        public const string INDEX_MAP_PARTITION_KEY = "indexmap_partition_key";
        public const string INDEX_MAP_ROW_KEY = "indexmap_row_key";
        public const string INDEX_PROPERTY_NAME = "index";

        private TableClient _tableClient;

        public IndexMapperService(IndexMapperOptions options) 
        {
            _tableClient = new TableClient(
                new Uri(options.TableStorageUri),
                INDEX_MAP_TABLE_NAME,
                new TableSharedKeyCredential(options.TableStorageAccountName, options.TableStorageKey),
                new TableClientOptions()
                {
                    Retry =
                    {
                        MaxRetries = 5,
                        Delay = TimeSpan.FromSeconds(2),
                        Mode = Azure.Core.RetryMode.Exponential
                    }
                });
        }

        public async Task SetIndexName(string name)
        {
            var table = await _tableClient.CreateIfNotExistsAsync();
            var indexMapEntity = new TableEntity(INDEX_MAP_PARTITION_KEY, INDEX_MAP_ROW_KEY)
            {
                { INDEX_PROPERTY_NAME, name }
            };

            try
            {
                await _tableClient.UpsertEntityAsync(indexMapEntity);
            }
            catch (RequestFailedException e)
            {
                throw new FailedToSetIndexException($"Failed to set {name} as the new index", e);
            }
        }

        public async Task<string> GetIndexName()
        {
            try
            {
                var entityResponse = await _tableClient.GetEntityAsync<TableEntity>(INDEX_MAP_PARTITION_KEY, INDEX_MAP_ROW_KEY);
                var entity = entityResponse.Value;
                return entity[INDEX_PROPERTY_NAME].ToString();
            }
            catch (RequestFailedException e)
            {
                throw new FailedToGetIndexException($"Failed to get index name", e);
            }
        }
    }
}
