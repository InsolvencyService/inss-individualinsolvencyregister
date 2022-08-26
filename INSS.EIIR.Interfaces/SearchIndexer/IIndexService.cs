namespace INSS.EIIR.Interfaces.SearchIndexer;

public interface IIndexService
{
    Task CreateIndexAsync();

    Task DeleteIndexAsync();

    Task PopulateIndexAsync();
}