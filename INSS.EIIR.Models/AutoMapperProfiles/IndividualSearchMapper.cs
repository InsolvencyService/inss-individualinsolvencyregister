using AutoMapper;
using INSS.EIIR.Models.IndexModels;

namespace INSS.EIIR.Models.AutoMapperProfiles;

public class IndividualSearchMapper : Profile
{
    public IndividualSearchMapper()
    {
        CreateMap<SearchResult, IndividualSearch>()
            .ForMember(m => m.CaseNumber, opt => opt.MapFrom(s => s.CaseNo))
            .ForMember(m => m.Court, opt => opt.MapFrom(s => s.Court))
            .ForMember(m => m.FirstName, opt => opt.MapFrom(s => s.FirstName))
            .ForMember(m => m.FamilyName, opt => opt.MapFrom(s => s.Surname));
    }
}