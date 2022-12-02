using INSS.EIIR.Interfaces.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Web.Services;

public class ExtractService : IExtractService
{
    private readonly IClientService _clientService;

    public ExtractService(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<IActionResult> GetLatestExtractAsync(string subscriberId)
    {
        return await _clientService.GetAsync<IActionResult>($"eiir/{subscriberId}/downloads/latest", new List<string>());
    }
}