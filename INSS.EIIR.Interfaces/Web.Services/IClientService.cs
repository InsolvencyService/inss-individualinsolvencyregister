namespace INSS.EIIR.Interfaces.Web.Services;

public interface IClientService
{
    Task<TResult> PutAsync<TContent, TResult>(string url, TContent content);

    Task<TResult> PostAsync<TContent, TResult>(string url, TContent content);

    Task<TResult> GetAsync<TResult>(string url, IList<string> parameters);
}