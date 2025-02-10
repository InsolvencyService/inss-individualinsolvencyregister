using INSS.EIIR.AzureSearch.IndexMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.XML
{
    public static class ExistingBankruptciesServiceExtension
    {
        public static IServiceCollection AddExistingBankrupticesService(this IServiceCollection services, ExistingBankruptciesOptions options)
        {
            services.AddSingleton<IExistingBankruptciesService>(new ExistingBankruptciesService(options));

            return services;
        }

    }
}
