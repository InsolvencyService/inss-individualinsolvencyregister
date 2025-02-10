using INSS.EIIR.Models.BannerModels;
using System.Diagnostics.CodeAnalysis;

namespace INSS.EIIR.Interfaces.Web.Services
{

    public interface IBanner
    {
        Task<Banner> GetBannerAsync();

        Task<Banner> SetBannerAsync(Banner value);
    }
}
