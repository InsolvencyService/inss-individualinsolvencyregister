using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace INSS.EIIR.Functions.Functions
{
    public class ExtractJobServiceTrigger
    {
        private readonly ILogger<ExtractJobServiceTrigger> _logger;
        private readonly IExtractRepository _eiirRepository;
        private readonly IExtractService _extractService;

        public ExtractJobServiceTrigger(
            ILogger<ExtractJobServiceTrigger> log,
            IExtractRepository eiirRepository,
            IExtractService extractService)
        {
            _logger = log;
            _eiirRepository = eiirRepository;
            _extractService = extractService;
        }

        [FunctionName("ExtractJobServiceTrigger")]
        public async Task Run([ServiceBusTrigger("%servicebus:extractjobqueue%", Connection = "servicebus:subscriberconnectionstring")] ExtractJobMessage message)
        {
            var now = DateTime.Now;

            _logger.LogInformation($"ExtractJobServiceTrigger received message: {message} on {now}");

            try
            {
                await _extractService.GenerateSubscriberFile(message.ExtractFilename);

                _eiirRepository.UpdateExtractAvailable();
                
                _logger.LogInformation($"ExtractJobServiceTrigger ran succssfully on: {now} xml/zip file created with name: {message.ExtractFilename}");
            }
            catch (Exception ex)
            {
                var error = $"ExtractJobServiceTrigger failed on: {now} with : {ex}";
                _logger.LogError(error);
                throw new Exception(error);
            }
        }
    }
}
