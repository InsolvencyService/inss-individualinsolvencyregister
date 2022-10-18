using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Models.SubscriberModels;

namespace INSS.EIIR.Data.AutoMapperProfiles;

public class SubscriberMapper : Profile
{
    public SubscriberMapper()
    {
        CreateMap<SubscriberAccount, Subscriber>();
        CreateMap<SubscriberApplication, SubscriberDetail>();
        CreateMap<SubscriberContact, SubscriberEmailContact>();
    }
}
