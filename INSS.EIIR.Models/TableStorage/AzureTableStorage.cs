using Azure;
using Azure.Data.Tables;

namespace INSS.EIIR.Models.TableStorage;

// <summary>
/// Azure table storage entity.
/// </summary>
public abstract class AzureTableStorage : ITableEntity
{
    /// <summary>
    /// Partition key of the table row.
    /// </summary>
    public string PartitionKey { get; set; } = null!;

    /// <summary>
    /// Row key of the table row.
    /// </summary>
    public string RowKey { get; set; } = null!;

    /// <summary>
    /// The created or updated datetimestamp on the row.
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// The http etag of the row.
    /// </summary>
    public ETag ETag { get; set; }
}
