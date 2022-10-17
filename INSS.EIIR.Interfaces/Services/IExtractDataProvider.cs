namespace INSS.EIIR.Interfaces.Services;

public interface IExtractDataProvider
{
    Task GenerateSubscriberFile(string filename);
}
