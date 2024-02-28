using System.Net.Mime;
using Flurl;
using Flurl.Http;
using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.Web.Services;

public class ExtractService : IExtractService
{
    private readonly ApiSettings _settings;

    public ExtractService(IOptions<ApiSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<IActionResult> GetLatestExtractAsync(string subscriberId)
    {
        var result = await _settings.BaseUrl
            .AppendPathSegment($"eiir/{subscriberId}/downloads/latest")
            .WithHeader("x-functions-key", _settings.ApiKey)
            .GetStreamAsync();

        byte[] bytes;
        using (var ms = new MemoryStream())
        {
            await result.CopyToAsync(ms);
            bytes = ms.ToArray();
        }

        return new FileContentResult(bytes, MediaTypeNames.Application.Zip);
    }
}