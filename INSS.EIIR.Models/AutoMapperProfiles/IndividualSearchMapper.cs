using AutoMapper;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Models.AutoMapperProfiles;

public class IndividualSearchMapper : Profile
{
    public IndividualSearchMapper()
    {
        CreateMap<IndividualSearch, SearchResult>()
            .ForMember(m => m.indvidualForenames, opt => opt.MapFrom(s => s.FirstName))
            .ForMember(m => m.indvidualSurname, opt => opt.MapFrom(s => s.FamilyName))
            .ForMember(m => m.indvidualAlias, opt => opt.MapFrom(s => s.AlternativeNames))
            .ForMember(m => m.companyName, opt => opt.MapFrom(s => s.CompanyName))
            .ForMember(m => m.indvidualTown, opt => opt.MapFrom(s => s.LastKnownLocality))
            .ForMember(m => m.indvidualPostcode, opt => opt.MapFrom(s => s.LastKnownPostcode))
            .ForMember(m => m.caseNo, opt => opt.MapFrom(s => s.CaseNumber))
            .ForMember(m => m.indivNo, opt => opt.MapFrom(s => s.IndividualNumber)).ReverseMap();
    }
}