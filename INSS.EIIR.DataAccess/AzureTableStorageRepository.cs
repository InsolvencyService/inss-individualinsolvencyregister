using Azure.Data.Tables;
using INSS.EIIR.Interfaces.Storage;
using INSS.EIIR.Models.TableStorage;
using System.Linq.Expressions;

namespace INSS.EIIR.DataAccess;

public class AzureTableStorageRepository<TEntity> : ITableStorageRepository<TEntity> where TEntity : AzureTableStorage, new()
{
    private TableClient _tableClient { get; set; }

    public AzureTableStorageRepository(TableServiceClient tableserviceClient)
    {
        var tableName = typeof(TEntity).Name;
        tableserviceClient.CreateTableIfNotExists(tableName);
        _tableClient = tableserviceClient.GetTableClient(tableName);
    }

    public async Task<TEntity> GetEntity(string partitionKey, string rowKey)
    {
        return await _tableClient.GetEntityAsync<TEntity>(partitionKey, rowKey);
    }

    public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
    {
        return _tableClient.QueryAsync(filter).ToEnumerable();
    }

    public async Task AddEntity(TEntity entity)
    {
        entity.RowKey = TicksKey();
        entity.PartitionKey = string.Empty;
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task DeleteEntity(string partitionKey, string rowKey)
    {
        await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task UpsertEntity(TEntity entity)
    {
        await _tableClient.UpsertEntityAsync(entity);
    }

    private static string TicksKey()
    {
        return (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks).ToString("d19");
    }
}
