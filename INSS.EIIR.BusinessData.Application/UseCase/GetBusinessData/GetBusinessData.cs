using INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData.Infrastructure;
using INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData.Model;
using INSS.EIIR.DataSync.Application;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData
{
    public class GetBusinessData : IResponseUseCase<GetBusinessDataResponse>
    {
        private readonly IGetBusinessData _getBusinessDataService;

        public GetBusinessData(IGetBusinessData getBusinessDataService)
        {

            _getBusinessDataService = getBusinessDataService;
        }


        public async Task<GetBusinessDataResponse> Handle()
        {
            return new GetBusinessDataResponse() { Data = await _getBusinessDataService.GetBusinessData()};
        }
    }
}
