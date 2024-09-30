using System;
using System.Collections.Generic;
using AutoMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Infrastructure.Fake.Source;
using INSS.EIIR.DataSync.Infrastructure.Sink.AISearch;
using INSS.EIIR.DataSync.Infrastructure.Sink.Failure;
using INSS.EIIR.DataSync.Infrastructure.Sink.XML;
using INSS.EIIR.DataSync.Infrastructure.Source.AzureTable;
using INSS.EIIR.DataSync.Infrastructure.Source.SQL;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.StubbedTestData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.DataSync.Functions.DI
{
    public static class SyncDataFactory
    {
        public static SyncData Get(IServiceProvider sp)
        {
            var factory = sp.GetRequiredService<ILoggerFactory>();
            var config = sp.GetRequiredService<IConfiguration>();
            var mapper = sp.GetRequiredService<IMapper>();
            var extractRepo = sp.GetRequiredService<IExtractRepository>();

            IEnumerable<IDataSourceAsync<InsolventIndividualRegisterModel>> sources;

            var useFakeData = config.GetValue<Boolean>("UseFakedDataSources", false);

            if (useFakeData)
            {
                sources = new List<IDataSourceAsync<InsolventIndividualRegisterModel>>()
                {
                    GetInsSightFakeDataSource(config, mapper),
                    GetEIIRSQLSourceFake(config, mapper)
                };
            }
            else
            {
                sources = new List<IDataSourceAsync<InsolventIndividualRegisterModel>>()
                {
                    GetINSSightSQLSource(config, mapper),
                    GetEIIRSQLSource(config, mapper)
                };
            }

            IEnumerable<IDataSink<InsolventIndividualRegisterModel>> sinks = new List<IDataSink<InsolventIndividualRegisterModel>>()
            {
                //Following lines are required.. eventually, they are commented as XMLSink and AISearchSink are yet to be implemented
                //and will crash calling function if deployed myself and Carl are actively working on this code in coming days
                GetXMLSink(config, extractRepo),
                //GetAISearchSink(config)
            };

            IEnumerable<ITransformRule> transformRules = new List<ITransformRule>();           

            var failureSinkOptions = new FailureSinkOptions();
            var failureSink = new FailureSink(failureSinkOptions);

            var options = new SyncDataOptions()
            {
                DataSources = sources,
                DataSinks = sinks,
                TransformRules = transformRules,
                FailureSink = failureSink
            };

            return new SyncData(options, factory.CreateLogger<SyncData>());
        }

        private static IDataSink<InsolventIndividualRegisterModel> GetXMLSink(IConfiguration config, IExtractRepository repo)
        {
            var options = new XMLSinkOptions() 
            { 
                StorageName = config.GetValue<String>("XmlContainer", null),
                StoragePath = config.GetValue<String>("TargetBlobConnectionString", null)
            };

            return new XMLSink(options, repo);
        }

        private static IDataSink<InsolventIndividualRegisterModel> GetAISearchSink(IConfiguration config)
        {
            var options = new AISearchSinkOptions();

            return new AISearchSink(options);
        }

        private static IDataSource GetAzureTableSource(IConfiguration config)
        {
            var options = new AzureTableSourceOptions();

            return new AzureTableSource(options);
        }
        private static IDataSourceAsync<InsolventIndividualRegisterModel> GetINSSightSQLSource(IConfiguration config, IMapper mapper)
        {
            var options = new SQLSourceOptions(mapper, config.GetValue<string>("InsSightSQLConnectionString"));

            return new INSSightSQLSource(options);
        }

        private static IDataSourceAsync<InsolventIndividualRegisterModel> GetEIIRSQLSource(IConfiguration config, IMapper mapper)
        {
            var options = new SQLSourceOptions(mapper, config.GetConnectionString("InsSightSQLConnectionString"));

            return new EiirSQLSource(options);
        }

        private static IDataSourceAsync<InsolventIndividualRegisterModel> GetEIIRSQLSourceFake(IConfiguration config, IMapper mapper)
        {
            return new EIIRSQLSourceFake(mapper);
        }

        private static IDataSourceAsync<InsolventIndividualRegisterModel> GetInsSightFakeDataSource(IConfiguration config, IMapper mapper)
        {
            return new InsSightSQLSourceFake(mapper);
        }
    }
}
