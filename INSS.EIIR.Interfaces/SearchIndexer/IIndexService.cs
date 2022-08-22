namespace INSS.EIIR.Interfaces.SearchIndexer;

public interface IIndexService
{
    void CreateIndex<T>(string indexName);
}