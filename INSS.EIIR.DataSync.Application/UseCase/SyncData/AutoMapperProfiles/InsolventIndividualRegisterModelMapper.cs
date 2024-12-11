using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models;
using Azure.Search.Documents.Indexes.Models;

namespace INSS.EIIR.DataSync.Application.UseCase.SyncData.AutoMapperProfiles
{
    public class InsolventIndividualRegisterModelMapper : Profile
    {

        public InsolventIndividualRegisterModelMapper()
        {
            CreateMap<CaseResult, InsolventIndividualRegisterModel>()
                //APP-5472 fix for 8 bit encoding issue
                .ForMember<string>(dest => dest.individualForenames, opt => opt.MapFrom(src => IirEncodingHelper.FixSQLEncoding(src.individualForenames)))
                .ForMember<string>(dest => dest.individualSurname, opt => opt.MapFrom(src => IirEncodingHelper.FixSQLEncoding(src.individualSurname)))
                .ForMember<string>(dest => dest.caseName, opt => opt.MapFrom(src => IirEncodingHelper.FixSQLEncoding(src.caseName)))
                .ForMember<string>(dest => dest.caseDescription, opt => opt.MapFrom(src => IirEncodingHelper.FixSQLEncoding(src.caseDescription)))
                //CaseResult historically did not contain annulDate and annulReason derived from caseStatus
                .ForMember(dest => dest.annulDate, opt => opt.MapFrom(src => src.caseStatus != null && src.caseStatus.StartsWith("ANNULLED", StringComparison.OrdinalIgnoreCase) ? new string(src.caseStatus.TakeLast(10).ToArray()) : null))
                .ForMember(dest => dest.annulReason, opt => opt.MapFrom(src => src.caseStatus != null && src.caseStatus.StartsWith("ANNULLED", StringComparison.OrdinalIgnoreCase) ? new string(src.caseStatus.Take(src.caseStatus.Length - 14).ToArray()) : null))
                .ReverseMap();


        }

    }
}
