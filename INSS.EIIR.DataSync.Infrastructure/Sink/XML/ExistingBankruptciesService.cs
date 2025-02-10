using Azure;
using Azure.Data.Tables;
using INSS.EIIR.AzureSearch.IndexMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public class ExistingBankruptciesService : IExistingBankruptciesService
    {
        public const string EX_BANK_TABLE_NAME = "ExistingBankruptcies";
        public const string EX_BANK_PARTITION_KEY = "existingbankruptcies_partition_key";
        public const string EX_BANK_ROW_KEY = "existingbankruptcies_row_key";
        public const string EX_BANK_PROPERTY_NAME = "bankruptcy_ids_json";


        private TableClient _tableClient;

        public ExistingBankruptciesService(ExistingBankruptciesOptions options) 
        {
            _tableClient = new TableClient(
                options.TableStorageConnectionString,
                EX_BANK_TABLE_NAME,
                new TableClientOptions()
                {
                    Retry =
                    {
                        MaxRetries = 5,
                        Delay = TimeSpan.FromSeconds(2),
                        Mode = Azure.Core.RetryMode.Exponential
                    }
                }
            );
        }

        async Task<SortedList<int, int>> IExistingBankruptciesService.GetExistingBankruptcies()
        {
            var defaultReturn = new SortedList<int, int>(); 

            try
            {
                var entityResponse = await _tableClient.GetEntityAsync<TableEntity>(EX_BANK_PARTITION_KEY, EX_BANK_ROW_KEY);
                var entity = entityResponse.Value;
                return JsonSerializer.Deserialize<SortedList<int, int>>(entity[EX_BANK_PROPERTY_NAME].ToString());
            }
            //If entity doesn't exist return the default
            catch (RequestFailedException)
            {
                return defaultReturn;
            }
        }

        async Task IExistingBankruptciesService.SetExistingBankruptcies(SortedList<int, int> existingBankruptcies)
        {
            var table = await _tableClient.CreateIfNotExistsAsync();
            var indexMapEntity = new TableEntity(EX_BANK_PARTITION_KEY, EX_BANK_ROW_KEY)
            {
                { EX_BANK_PROPERTY_NAME, JsonSerializer.Serialize(existingBankruptcies) }
            };

            try
            {
                await _tableClient.UpsertEntityAsync(indexMapEntity);
            }
            catch (RequestFailedException)
            {
                throw new Exception($"Failed to save existing bankruptcy identifiers");
            }
        }
    }
}
