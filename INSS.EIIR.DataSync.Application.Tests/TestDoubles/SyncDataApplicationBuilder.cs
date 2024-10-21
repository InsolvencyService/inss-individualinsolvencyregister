using INSS.EIIR.DataSync.Application.UseCase.SyncData;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Validation;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Interfaces.DataAccess;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace INSS.EIIR.DataSync.Application.Tests.TestDoubles
{
    public class SyncDataApplicationBuilder
    {
        private readonly List<IDataSourceAsync<InsolventIndividualRegisterModel>> _dataSources = new List<IDataSourceAsync<InsolventIndividualRegisterModel>>();
        private readonly List<IDataSink<InsolventIndividualRegisterModel>> _dataSinks = new List<IDataSink<InsolventIndividualRegisterModel>>();   
        private readonly List<ITransformRule> _rules = new List<ITransformRule>();
        private readonly List<IValidationRule> _validationrules = new List<IValidationRule>();
        private IDataSink<SyncFailure> _failureSink = Substitute.For<IDataSink<SyncFailure>>();
        private ILogger<SyncData>? _logger;
        private IExtractRepository _extractRepo = Substitute.For<IExtractRepository>();

        public static SyncDataApplicationBuilder Create()
        {
            return new SyncDataApplicationBuilder();
        }

        public SyncDataApplicationBuilder WithDataSource(IDataSourceAsync<InsolventIndividualRegisterModel> dataSource) 
        {
            _dataSources.Add(dataSource);
            return this;
        }

        public SyncDataApplicationBuilder WithDataSink(IDataSink<InsolventIndividualRegisterModel> dataSink)
        {
            _dataSinks.Add(dataSink);
            return this;
        }

        public SyncDataApplicationBuilder WithFailureSink(IDataSink<SyncFailure> dataSink)
        {
            _failureSink = dataSink;
            return this;
        }

        public SyncDataApplicationBuilder WithTransformationRule(ITransformRule rule) 
        {
            _rules.Add(rule);
            return this;
        }

        public SyncDataApplicationBuilder WithValidationRule(IValidationRule rule)
        {
            _validationrules.Add(rule);
            return this;
        }

        public SyncDataApplicationBuilder WithExtractRepo(IExtractRepository extractRepo)
        {
            _extractRepo = extractRepo;
            return this;
        }

        public SyncDataApplicationBuilder WithLogger(ILogger<SyncData> logger)
        {
            _logger = logger;
            return this;
        }

        public SyncData Build()
        {
            // if we didn't call it we don't care but it is necessary...
            if (_logger == null)
            {
                _logger = Substitute.For<ILogger<SyncData>>();
            }

            var options = new SyncDataOptions()
            {
                DataSources = _dataSources,
                DataSinks = _dataSinks,
                TransformRules = _rules,
                ValidationRules = _validationrules,
                FailureSink = _failureSink
            };

            return new SyncData(options, _extractRepo, _logger);
        }
    }
}
