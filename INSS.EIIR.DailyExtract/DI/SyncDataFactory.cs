using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Markup;
using AutoMapper;
using INSS.EIIR.AzureSearch.IndexMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;
using INSS.EIIR.DataSync.Infrastructure.Fake.Source;
using INSS.EIIR.DataSync.Infrastructure.Sink.AISearch;
using INSS.EIIR.DataSync.Infrastructure.Sink.Failure;
using INSS.EIIR.DataSync.Infrastructure.Sink.XML;
using INSS.EIIR.DataSync.Infrastructure.Source.AzureTable;
using INSS.EIIR.DataSync.Infrastructure.Source.SQL;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.SyncData;
using INSS.EIIR.StubbedTestData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace INSS.EIIR.DataSync.Functions.DI
{
    public static class SyncDataFactory
    {
        public const string AI_SEARCH_ENDPOINT_SETTING = "AISearchEndpoint";
        public const string AI_SEARCH_KEY_SETTING = "AISearchKey";
        public const string AI_SEARCH_BATCH_LIMIT_SETTING = "AISearchBatchLimit";

        public static SyncData Get(IServiceProvider sp)
        {
            var factory = sp.GetRequiredService<ILoggerFactory>();
            var config = sp.GetRequiredService<IConfiguration>();
            var mapper = sp.GetRequiredService<IMapper>();
            var indexMapper = sp.GetRequiredService<ISetIndexMapService>();
            var extractRepo = sp.GetRequiredService<IExtractRepository>();
            var exBankruptcyService = sp.GetRequiredService<IExistingBankruptciesService>();
            var validationRules = sp.GetRequiredService<IEnumerable<IValidationRule>>();
            var transformRules = sp.GetRequiredService<IEnumerable<ITransformRule>>();


            var permittedDataSources = GetPermittedDataSources(config);

            //Datasources and selected in SyncData via their Type property
            IEnumerable<IDataSourceAsync<InsolventIndividualRegisterModel>> sources = new List<IDataSourceAsync<InsolventIndividualRegisterModel>>()
            {
                //Fake Data Sources
                GetInsSightFakeDataSource(config, mapper),
                GetEIIRSQLSourceFake(config, mapper),

                //ISCIS Data Sources
                GetEIIRSQLSource(config, mapper),
                GetEIIRLocalSQLIVAB(config, mapper),

                //INSSight Data Sources
                GetINSSightSQLSource(config, mapper)
            };


            IEnumerable<IDataSink<InsolventIndividualRegisterModel>> sinks = new List<IDataSink<InsolventIndividualRegisterModel>>()
            {
                GetAISearchSink(config, mapper, indexMapper, factory.CreateLogger<AISearchSink>(), permittedDataSources),
                GetXMLSink(config, extractRepo, exBankruptcyService, permittedDataSources)               
            };

            var failureSinkOptions = new FailureSinkOptions();
            var failureSink = new FailureSink(factory.CreateLogger<FailureSink>(), failureSinkOptions);

            var options = new SyncDataOptions()
            {
                PermittedDataSources = permittedDataSources,
                DataSources = sources,
                DataSinks = sinks,
                TransformRules = transformRules,
                ValidationRules = validationRules,
                FailureSink = failureSink
            };

            return new SyncData(options, extractRepo, factory.CreateLogger<SyncData>());
        }

        private static SyncDataEnums.Datasource GetPermittedDataSources(IConfiguration config)
        {
            var setting = config.GetValue<object>("PermittedDataSources", 0);

            SyncDataEnums.Datasource value = 0;

            if (!Enum.TryParse(setting.ToString(),true,out value))
            {
                value = setting.ToString().Split('|').ToList().Select(e =>
                {
                    SyncDataEnums.Datasource u;
                    Enum.TryParse(e.Trim(), true, out u);
                    return u;
                }).Aggregate((u, c) => u = u | c);
            }
            return value;
        }

        private static IDataSink<InsolventIndividualRegisterModel> GetXMLSink(IConfiguration config, IExtractRepository repo, IExistingBankruptciesService service, SyncDataEnums.Datasource permittedDataSources)
        {
            var options = new XMLSinkOptions()
            {
                PermittedDataSources = permittedDataSources,
                StorageName = config.GetValue<String>("XmlContainer", null),
                StoragePath = config.GetValue<String>("TargetBlobConnectionString", null),
                WriteToBlobRecordBufferSize = config.GetValue<int>("SyncDataWriteXMLBufferSize", 500)
            };

            return new XMLSink(options, repo, service);
        }

        private static IDataSink<InsolventIndividualRegisterModel> GetAISearchSink(IConfiguration config, IMapper mapper, ISetIndexMapService indexMapper, ILogger<AISearchSink> logger, SyncDataEnums.Datasource permittedDataSources)
        {
            var options = new AISearchSinkOptions();
            options.PermittedDataSources = permittedDataSources;
            options.AISearchEndpoint = config.GetValue<string>(AI_SEARCH_ENDPOINT_SETTING);
            options.AISearchKey = config.GetValue<string>(AI_SEARCH_KEY_SETTING);
            options.BatchLimit = config.GetValue<int>(AI_SEARCH_BATCH_LIMIT_SETTING);
            options.Mapper = mapper;

            return new AISearchSink(options, indexMapper, logger);
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
            var options = new SQLSourceOptions(mapper, config.GetValue<string>("database:connectionstring"));

            return new EiirSQLSource(options);
        }

        /// <summary>
        /// Temporary method to allow localised testing of INSSight Integration functionality
        /// To be removed once INSSight feed is available
        /// </summary>
        private static IDataSourceAsync<InsolventIndividualRegisterModel> GetEIIRLocalSQLIVAB(IConfiguration config, IMapper mapper)
        {
            var options = new SQLSourceOptions(mapper, config.GetValue<string>("database:connectionstring"));

            return new EIIRLocalSQLIVAB(options);
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
