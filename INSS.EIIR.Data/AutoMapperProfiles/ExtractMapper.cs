using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Models.ExtractModels;

namespace INSS.EIIR.Models.AutoMapperProfiles;

public class ExtractMapper : Profile
{
    public ExtractMapper()
    {

        CreateMap<ExtractAvailabilitySP, ExtractAvailable>();
    }
}