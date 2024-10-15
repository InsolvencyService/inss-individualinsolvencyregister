using INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData.Model;
using INSS.EIIR.DataSync.Application;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData
{
    public class GetBusinessData : IResponseUseCase<GetBusinessDataResponse>
    {
        public Task<GetBusinessDataResponse> Handle()
        {
            return Task.FromResult(new GetBusinessDataResponse() { Data = new Model.BusinessData() { BannerText = "John likes to party" } });
        }
    }
}
