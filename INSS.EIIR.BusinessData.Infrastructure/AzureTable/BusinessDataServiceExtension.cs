using INSS.EIIR.BusinessData.Application.UseCase.GetBusinessData.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.BusinessData.Infrastructure.AzureTable
{
    public static class BusinessDataServiceExtension
    {

        public static IServiceCollection AddBusinessDataService(this IServiceCollection services, BusinessDataServiceOptions options)
        {
            services.AddSingleton<IGetBusinessData>(new BusinessDataService(options));

            return services;
        }

    }
}
