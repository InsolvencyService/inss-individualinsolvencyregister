using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.Constants;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.Failure
{
    public class FailureSink : IDataSink<SyncFailure>
    {
        //Make the failure Sink the default? logger => Applciation Insights
        private readonly ILogger<FailureSink> _logger;

        //Property never referenced for IDataSink<SyncFailure> though required by IDataSink
        public SyncData.Mode EnabledCheckBit => throw new NotImplementedException();

        public string Description => "Sink for logging failures";

        public FailureSink(ILogger<FailureSink> logger, FailureSinkOptions options)
        {
            this._logger = logger;
        }

        public async Task Start() { return; }


        public async Task<SinkCompleteResponse> Complete(bool commit = true)
        {
            return new SinkCompleteResponse() { IsError = false };
        }

        public async Task<DataSinkResponse> Sink(SyncFailure model)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var str in model.ErrorMessages) 
            {
                stringBuilder.AppendLine(str);
            }

            _logger.LogError(stringBuilder.ToString());

            return new DataSinkResponse() { IsError = false };
        }
    }
}
