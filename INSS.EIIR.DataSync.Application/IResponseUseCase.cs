using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application
{
    public interface IResponseUseCase<TResponse>
    {
        Task<TResponse> Handle();
    }
}
