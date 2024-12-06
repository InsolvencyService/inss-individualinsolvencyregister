using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.AutoMapperProfiles
{
    public class InsolventIndividualRegisterModelMapper : Profile
    {

        public InsolventIndividualRegisterModelMapper()
        {
            CreateMap<CaseResult, InsolventIndividualRegisterModel>()
                //CaseResult historically did not contain annulDate and annulReason derived from caseStatus
                .ForMember(dest => dest.annulDate, opt => opt.MapFrom(src => src.caseStatus != null && src.caseStatus.StartsWith("ANNULLED", StringComparison.OrdinalIgnoreCase) ? src.caseStatus.TakeLast(10) : null))
                .ForMember(dest => dest.annulReason, opt => opt.MapFrom(src => src.caseStatus != null && src.caseStatus.StartsWith("ANNULLED", StringComparison.OrdinalIgnoreCase) ? src.caseStatus.Take(src.caseStatus.Length-14) : null))
                .ReverseMap();
        }

    }
}
