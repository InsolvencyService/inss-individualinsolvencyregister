using INSS.EIIR.DataSync.Application.UseCase.GetBanner.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.GetBanner
{
    public class GetBusinessData : IResponseUseCase<GetBusinessDataResponse>
    {
        public Task<GetBusinessDataResponse> Handle()
        {
            return Task.FromResult(new GetBusinessDataResponse() { BannerText = "John likes to party" });
        }
    }
}
