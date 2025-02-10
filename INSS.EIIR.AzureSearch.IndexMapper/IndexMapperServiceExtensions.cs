using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.AzureSearch.IndexMapper
{
    public static class IndexMapperServiceExtensions
    {
        public static IServiceCollection AddSetIndexMapper(this IServiceCollection services, IndexMapperOptions options)
        {
            services.AddSingleton<ISetIndexMapService>(new IndexMapperService(options));

            return services;
        }

        public static IServiceCollection AddGetIndexMapper(this IServiceCollection services, IndexMapperOptions options)
        {
            services.AddSingleton<IGetIndexMapService>(new IndexMapperService(options));

            return services;
        }
    }
}
