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
                .ReverseMap();
        }

    }
}
