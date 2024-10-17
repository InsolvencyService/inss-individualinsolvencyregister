using INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData.Infrastructure;
using INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData.Model;
using INSS.EIIR.DataSync.Application;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData
{
    public class ManageBusinessData : IResponseUseCase<GetBusinessDataResponse>, IRequestResponseUseCase<SetBusinessDataRequest, SetBusinessDataResponse>
    {
        private readonly IGetBusinessData _getBusinessDataService;
        private readonly ISetBusinessData _setBusinessDataService;

        public ManageBusinessData(IGetBusinessData getBusinessDataService, ISetBusinessData setBusinessDataService)
        {
            _getBusinessDataService = getBusinessDataService;
            _setBusinessDataService = setBusinessDataService;
        }

        public async Task<GetBusinessDataResponse> Handle()
        {
            return new GetBusinessDataResponse() { Data = await _getBusinessDataService.GetBusinessData()};
        }

        public async Task<SetBusinessDataResponse> Handle(SetBusinessDataRequest request)
        {
            return new SetBusinessDataResponse() { Data = await _setBusinessDataService.SetBusinessData(request.Data) };
        }
    }
}
