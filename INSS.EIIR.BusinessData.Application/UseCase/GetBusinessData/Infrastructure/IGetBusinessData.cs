using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData.Model;

namespace INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData.Infrastructure
{
    public interface IGetBusinessData
    {
        Task<Model.BusinessData> GetBusinessData();
    }
}
