using Azure;
using Azure.Data.Tables;
using INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData.Infrastructure;
using INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INSS.EIIR.BusinessData.Infrastructure.AzureTable
{
    public class BusinessDataService : IGetBusinessData, ISetBusinessData
    {
        public const string BUSINESS_DATA_TABLE_NAME = "BusinessData";
        public const string BUSINESS_DATA_PARTITION_KEY = "businessdata_partition_key";
        public const string BUSINESS_DATA_ROW_KEY = "businessdata_row_key";
        public const string BUSINESS_DATA_PROPERTY_NAME = "json_businessdata";

        private TableClient _tableClient;

        public BusinessDataService(BusinessDataServiceOptions options)
        {
            _tableClient = new TableClient(
                options.TableStorageConnectionString,
                BUSINESS_DATA_TABLE_NAME,
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

        public async Task<Application.UseCase.ManageBusinessData.Model.BusinessData> GetBusinessData()
        {
            var defaultReturn = new Application.UseCase.ManageBusinessData.Model.BusinessData();

            try
            {
                var entityResponse = await _tableClient.GetEntityAsync<TableEntity>(BUSINESS_DATA_PARTITION_KEY, BUSINESS_DATA_ROW_KEY);
                var entity = entityResponse.Value;
                return JsonSerializer.Deserialize<Application.UseCase.ManageBusinessData.Model.BusinessData>(entity[BUSINESS_DATA_PROPERTY_NAME].ToString());
            }
            catch (RequestFailedException e)
            {
                return defaultReturn;
            }

        }

        public async Task<Application.UseCase.ManageBusinessData.Model.BusinessData> SetBusinessData(Application.UseCase.ManageBusinessData.Model.BusinessData value)
        {
            var table = await _tableClient.CreateIfNotExistsAsync();
            var entity = new TableEntity(BUSINESS_DATA_PARTITION_KEY, BUSINESS_DATA_ROW_KEY)
            {
                { BUSINESS_DATA_PROPERTY_NAME, JsonSerializer.Serialize(value) }
            };

            try
            {
                var resp = await _tableClient.UpsertEntityAsync(entity);
                return value;
            }
            catch (RequestFailedException)
            {
                throw new Exception($"Failed to save business data");
            }
        }
    }
}
