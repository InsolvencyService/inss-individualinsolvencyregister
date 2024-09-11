using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application
{
    public interface IRequestResponseUseCase<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
