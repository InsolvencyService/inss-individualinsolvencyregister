using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.BannerModels;
using INSS.EIIR.Models.SearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Web.Services
{
    public class BannerService : IBanner
    {
        private const string ApiUrl = "eiir/banner";

        private readonly IClientService _clientService;

        public BannerService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Banner> GetBannerAsync()
        {
            var result = await _clientService.GetAsync<Banner>(ApiUrl, new List<string>());

            return result;
        }


    }
}
