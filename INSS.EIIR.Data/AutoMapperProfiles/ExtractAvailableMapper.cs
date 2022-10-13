using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Models.ExtractModels;

namespace INSS.EIIR.Models.AutoMapperProfiles;

public class ExtractAvailableMapper : Profile
{
    public ExtractAvailableMapper()
    {

        CreateMap<ExtractAvailabilitySP, ExtractAvailable>();
    }
}