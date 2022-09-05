using AutoMapper;
using INSS.EIIR.Models;
using INSS.EIIR.Models.IndexModels;

namespace INSS.EIIR.AzureSearch.AutoMapperProfiles;

public class IndividualSearchMapper : Profile
{
    public IndividualSearchMapper()
    {
        CreateMap<SearchResult, IndividualSearch>()
            .ForMember(m => m.FamilyName, opt => opt.MapFrom(s => s.Surname));
    }
}