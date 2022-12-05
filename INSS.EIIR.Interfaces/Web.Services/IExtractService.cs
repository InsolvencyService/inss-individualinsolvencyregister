using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.Interfaces.Web.Services;

public interface IExtractService
{
    Task<IActionResult> GetLatestExtractAsync(string subscriberId);
}