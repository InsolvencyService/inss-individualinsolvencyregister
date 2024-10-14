using INSS.EIIR.Models.BannerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Interfaces.Web.Services
{
    public interface IBanner
    {
        Task<Banner> GetBannerAsync();
    }
}
