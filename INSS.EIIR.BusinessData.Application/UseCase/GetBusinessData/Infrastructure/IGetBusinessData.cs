using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData.Model;

namespace INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData.Infrastructure
{
    public interface IGetBusinessData
    {
        Task<Model.BusinessData> GetBusinessData();
    }
}
