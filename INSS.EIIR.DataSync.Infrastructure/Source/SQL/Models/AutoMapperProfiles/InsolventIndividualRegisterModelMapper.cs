using AutoMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.DataSync.Infrastructure.Source.SQL.Models.AutoMapperProfiles
{
    public class InsolventIndividualRegisterModelMapper : Profile
    {
        public InsolventIndividualRegisterModelMapper()
        {
            CreateMap<Models.VwEiir, InsolventIndividualRegisterModel>()
            .ForMember(dest => dest.individualForenames, opt => opt.MapFrom(src => src.IndividualForenames))
            .ForMember(dest => dest.individualSurname, opt => opt.MapFrom(src => src.IndividualSurname))
            .ForMember(dest => dest.individualTitle, opt => opt.MapFrom(src => src.IndividualTitle))
            .ForMember(dest => dest.individualAlias, opt => opt.MapFrom(src => src.IndividualAlias))
            .ForMember(dest => dest.individualGender, opt => opt.MapFrom(src => src.IndividualGender))
            .ForMember(dest => dest.individualDOB, opt => opt.MapFrom(src => src.IndividualDob.Trim()))
            .ForMember(dest => dest.individualOccupation, opt => opt.MapFrom(src => src.IndividualOccupation))
            .ForMember(dest => dest.individualTown, opt => opt.MapFrom(src => src.IndividualTown))
            .ForMember(dest => dest.individualAddress, opt => opt.MapFrom(src => src.IndividualAddress))
            .ForMember(dest => dest.individualPostcode, opt => opt.MapFrom(src => src.IndividualPostcode))
            .ForMember(dest => dest.individualAddressWithheld, opt => opt.MapFrom(src => src.IndividualAddressWithheld))
            .ForMember(dest => dest.caseName, opt => opt.MapFrom(src => src.CaseName))
            .ForMember(dest => dest.courtName, opt => opt.MapFrom(src => src.CourtName))
            .ForMember(dest => dest.courtNumber, opt => opt.MapFrom(src => src.CourtNumber))
            .ForMember(dest => dest.caseYear, opt => opt.MapFrom(src => src.CaseYear))
            .ForMember(dest => dest.insolvencyType, opt => opt.MapFrom(src => src.InsolvencyType))
            .ForMember(dest => dest.notificationDate, opt => opt.MapFrom(src => src.NotificationDate))
            .ForMember(dest => dest.caseStatus, opt => opt.MapFrom(src => src.CaseStatus))
            .ForMember(dest => dest.caseDescription, opt => opt.MapFrom(src => src.CaseDescription))
            .ForMember(dest => dest.tradingNames, opt => opt.MapFrom(src => src.TradingNames))
            .ForMember(dest => dest.insolvencyDate, opt => opt.MapFrom(src => src.InsolvencyDate))
            .ForMember(dest => dest.hasRestrictions, opt => opt.MapFrom(src => src.HasRestrictions))
            .ForMember(dest => dest.restrictionsType, opt => opt.MapFrom(src => src.RestrictionsType))
            .ForMember(dest => dest.restrictionsStartDate, opt => opt.MapFrom(src => src.RestrictionsStartDate.HasValue ? src.RestrictionsStartDate.Value.ToDateTime(TimeOnly.MinValue):(DateTime?)null))
            .ForMember(dest => dest.restrictionsEndDate, opt => opt.MapFrom(src => src.RestrictionsEndDate.HasValue ? src.RestrictionsEndDate.Value.ToDateTime(TimeOnly.MinValue):(DateTime?)null))
            .ForMember(dest => dest.hasaPrevInterimRestrictionsOrder, opt => opt.MapFrom(src => src.HasaPrevInterimRestrictionsOrder))
            .ForMember(dest => dest.prevInterimRestrictionsOrderStartDate, opt => opt.MapFrom(src => src.PrevInterimRestrictionsOrderStartDate.HasValue ? src.PrevInterimRestrictionsOrderStartDate.Value.ToDateTime(TimeOnly.MinValue):(DateTime?)null))
            .ForMember(dest => dest.prevInterimRestrictionsOrderEndDate, opt => opt.MapFrom(src => src.PrevInterimRestrictionsOrderEndDate.HasValue ? src.PrevInterimRestrictionsOrderEndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
            .ForMember(dest => dest.insolvencyPractitionerName, opt => opt.MapFrom(src => src.InsolvencyPractitionerName))
            .ForMember(dest => dest.insolvencyPractitionerFirmName, opt => opt.MapFrom(src => src.InsolvencyPractitionerFirmName))
            .ForMember(dest => dest.insolvencyPractitionerAddress, opt => opt.MapFrom(src => src.InsolvencyPractitionerAddress))
            .ForMember(dest => dest.insolvencyPractitionerPostcode, opt => opt.MapFrom(src => src.InsolvencyPractitionerPostcode))
            .ForMember(dest => dest.insolvencyPractitionerTelephone, opt => opt.MapFrom(src => src.InsolvencyPractitionerTelephone))
            .ForMember(dest => dest.insolvencyServiceOffice, opt => opt.MapFrom(src => src.InsolvencyServiceOffice))
            .ForMember(dest => dest.insolvencyServiceContact, opt => opt.MapFrom(src => src.InsolvencyServiceContact))
            .ForMember(dest => dest.insolvencyServiceAddress, opt => opt.MapFrom(src => src.InsolvencyServiceAddress))
            .ForMember(dest => dest.insolvencyServicePostcode, opt => opt.MapFrom(src => src.InsolvencyServicePostcode))
            .ForMember(dest => dest.insolvencyServicePhone, opt => opt.MapFrom(src => src.InsolvencyServicePhone))
            .ForMember(dest => dest.caseNo, opt => opt.MapFrom(src => src.CaseNo))
            .ForMember(dest => dest.indivNo, opt => opt.MapFrom(src => src.IndivNo))
            .ForMember(dest => dest.dateOfPreviousOrder, opt => opt.MapFrom(src => src.DateOfPreviousOrder.HasValue ? src.DateOfPreviousOrder.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
            .ForMember(dest => dest.deceasedDate, opt => opt.MapFrom(src => src.DeceasedDate))
            .ReverseMap()
            .ForPath(s => s.RestrictionsStartDate, opt => opt.MapFrom(src => src.restrictionsStartDate.HasValue ? DateOnly.FromDateTime(src.restrictionsStartDate.Value) : (DateOnly?)null))
            .ForPath(s => s.RestrictionsEndDate, opt => opt.MapFrom(src => src.restrictionsEndDate.HasValue ? DateOnly.FromDateTime(src.restrictionsEndDate.Value) : (DateOnly?)null))
            .ForPath(s => s.PrevInterimRestrictionsOrderStartDate, opt => opt.MapFrom(src => src.prevInterimRestrictionsOrderStartDate.HasValue ? DateOnly.FromDateTime(src.prevInterimRestrictionsOrderStartDate.Value) : (DateOnly?)null))
            .ForPath(s => s.PrevInterimRestrictionsOrderEndDate, opt => opt.MapFrom(src => src.prevInterimRestrictionsOrderEndDate.HasValue ? DateOnly.FromDateTime(src.prevInterimRestrictionsOrderEndDate.Value) : (DateOnly?)null))
            .ForPath(s => s.DateOfPreviousOrder, opt => opt.MapFrom(src => src.dateOfPreviousOrder.HasValue ? DateOnly.FromDateTime(src.dateOfPreviousOrder.Value) : (DateOnly?)null));
        }

    }
}
