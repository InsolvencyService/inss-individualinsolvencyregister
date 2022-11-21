using System.Linq.Expressions;

namespace INSS.EIIR.Interfaces.Storage;

/// <summary>
/// Azure table storage repository.
/// </summary>
/// <typeparam name="TEntity">The table storage entity.</typeparam>
public interface ITableStorageRepository<TEntity>
{
    /// <summary>
    /// Gets a matched row from the azure storage table. 
    /// </summary>
    /// <param name="partitionKey">The partition key to match on.</param>
    /// <param name="rowKey">The row key to match on.</param>
    /// <returns>The matched table row.</returns>
    Task<TEntity> GetEntity(string partitionKey, string rowKey);

    /// <summary>
    /// Reurns the result of the query ran against the azure table storage.
    /// </summary>
    /// <param name="filter">The filter applied to the query.</param>
    /// <returns>The query result.</returns>
    IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter);

    /// <summary>
    /// Adds an entity to the azure table storage.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    /// <returns>The asynchronous task.</returns>
    Task AddEntity(TEntity entity);

    /// <summary>
    /// Deletes an entity to the azure table storage.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>The asynchronous task.</returns>
    Task DeleteEntity(string partitionKey, string rowKey);
}
