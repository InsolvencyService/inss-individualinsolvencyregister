using System;
using Flurl;
using System.Reflection.Metadata;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Web.Services;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.SubscriberModels;
using Microsoft.Extensions.Options;
using Flurl.Http;

namespace INSS.EIIR.Web.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ApiSettings _settings;

        public SubscriberService(IOptions<ApiSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<Subscriber> GetSubscriberByIdAsync(string subscriberId)
        {
            return await _settings.BaseUrl
                .AppendPathSegment("SubscriberById")
                .WithHeader("x-functions-key", _settings.ApiKey)
                .GetJsonAsync<Subscriber>();
        }

        public async Task UpdateSubscriberAsync(string subscriberId, CreateUpdateSubscriber subscriber)
        {
            await _settings.BaseUrl
                .AppendPathSegment("subscriber-update")
                .WithHeader("x-functions-key", _settings.ApiKey)
                .PostJsonAsync(subscriber);
        }
    }
}

