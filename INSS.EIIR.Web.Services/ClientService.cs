using Flurl;
using Flurl.Http;
using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Configuration;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.Web.Services;

public class ClientService : IClientService
{
    private readonly ApiSettings _settings;

    public ClientService(
        IOptions<ApiSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<TResult> PostAsync<TContent, TResult>(string url, TContent content)
    {
        return await _settings.BaseUrl
            .AppendPathSegment(url)
            .WithHeader("x-functions-key", _settings.ApiKey)
            .PostJsonAsync(content)
            .ReceiveJson<TResult>();
    }

    public async Task<TResult> GetAsync<TResult>(string url, IList<string> parameters)
    {
        var clientUrl = _settings.BaseUrl.AppendPathSegment(url);

        foreach (var parameter in parameters)
        {
            clientUrl.AppendPathSegment(parameter);
        }

        return await clientUrl.GetJsonAsync<TResult>();
    }
}