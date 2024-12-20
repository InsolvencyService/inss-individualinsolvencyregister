using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Models;
using INSS.EIIR.Models.FeedbackModels;

namespace INSS.EIIR.Data.AutoMapperProfiles
{
    public class FeedbackMapper : Profile
    {
        public FeedbackMapper()
        {
            CreateMap<CiCaseFeedback, CaseFeedback>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.InsolvencyDate));

            CreateMap<CreateCaseFeedback, CiCaseFeedback>();
        }
    }
}
