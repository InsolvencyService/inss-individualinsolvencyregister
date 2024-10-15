using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.BusinessData.Application.UseCase.ManageBusinessData.Infrastructure
{
    public interface ISetBusinessData
    {
        Task<Model.BusinessData> SetBusinessData(Model.BusinessData value);
    }
}
