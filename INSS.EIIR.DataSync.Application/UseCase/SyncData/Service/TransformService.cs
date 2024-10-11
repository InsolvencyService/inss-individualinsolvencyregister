using INSS.EIIR.DataSync.Application.UseCase.SyncData.Infrastructure;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.Service
{
    public class TransformService
    {
        private readonly IEnumerable<ITransformRule> _rules;

        public TransformService(IEnumerable<ITransformRule> rules) 
        {
            _rules = rules;
        }

        public async Task<TransformResponse> Transform(InsolventIndividualRegisterModel model)
        {
            InsolventIndividualRegisterModel transformedModel = model;

            var transformResponse = new TransformResponse()
            {
                ErrorMessages = new List<string>(),
                IsError = false
            };

            foreach (var rule in _rules)
            {
                var response = await rule.Transform(transformedModel);
                if (response.IsError)
                {
                    transformResponse.ErrorMessages.Add(response.ErrorMessage);
                    transformResponse.IsError = true;   
                }
                else
                {
                    model = response.Model;
                }
            }

            transformResponse.Model = transformedModel;

            return transformResponse;
        }
    }
}
