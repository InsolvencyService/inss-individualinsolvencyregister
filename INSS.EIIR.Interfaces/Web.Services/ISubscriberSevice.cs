﻿using System;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Interfaces.Web.Services
{
    public interface ISubscriberService
    {
        Task<Subscriber> GetSubscriberByIdAsync(string subscriberId);

        Task CreateSubscriberAsync(CreateUpdateSubscriber subscriber);

        Task UpdateSubscriberAsync(string subscriberId, CreateUpdateSubscriber subscriber);
    }
}

