using AutoMapper;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace INSS.EIIR.DataSync.Infrastructure.Sink.AISearch.AutoMapperProfiles
{
    public class IndividualSearchToInsolventIndividualMapper : Profile
    {
        public IndividualSearchToInsolventIndividualMapper()
        {
            CreateMap<InsolventIndividualRegisterModel, IndividualSearch>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.individualForenames))
            .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.individualSurname))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.individualTitle))
            .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.individualAlias))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.individualGender))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.individualDOB))
            .ForMember(dest => dest.Occupation, opt => opt.MapFrom(src => src.individualOccupation))
            .ForMember(dest => dest.LastKnownTown, opt => opt.MapFrom(src => src.individualTown))
            .ForMember(dest => dest.LastKnownAddress, opt => opt.MapFrom(src => src.individualAddress))
            .ForMember(dest => dest.LastKnownPostcode, opt => opt.MapFrom(src => src.individualPostcode))
            .ForMember(dest => dest.AddressWithheld, opt => opt.MapFrom(src => src.individualAddressWithheld))
            .ForMember(dest => dest.CaseName, opt => opt.MapFrom(src => src.caseName))
            .ForMember(dest => dest.Court, opt => opt.MapFrom(src => src.courtName))
            .ForMember(dest => dest.CourtNumber, opt => opt.MapFrom(src => src.courtNumber))
            .ForMember(dest => dest.CaseYear, opt => opt.MapFrom(src => src.caseYear))
            .ForMember(dest => dest.InsolvencyType, opt => opt.MapFrom(src => src.insolvencyType))
            .ForMember(dest => dest.NotificationDate, opt => opt.MapFrom(src => src.notificationDate))
            .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => src.caseStatus))
            .ForMember(dest => dest.CaseDescription, opt => opt.MapFrom(src => src.caseDescription))
            .ForMember(dest => dest.TradingData, opt => opt.MapFrom(src => src.tradingNames))
            .ForMember(dest => dest.InsolvencyDate, opt => opt.MapFrom(src => src.insolvencyDate))
            .ForMember(dest => dest.HasRestrictions, opt => opt.MapFrom(src => src.hasRestrictions))
            .ForMember(dest => dest.RestrictionsType, opt => opt.MapFrom(src => src.restrictionsType))
            .ForMember(dest => dest.RestrictionsStartDate, opt => opt.MapFrom(src => src.restrictionsStartDate))
            .ForMember(dest => dest.RestrictionsEndDate, opt => opt.MapFrom(src => src.restrictionsEndDate))
            .ForMember(dest => dest.HasPrevInterimRestrictionsOrder, opt => opt.MapFrom(src => src.hasaPrevInterimRestrictionsOrder))
            .ForMember(dest => dest.PrevInterimRestrictionsOrderStartDate, opt => opt.MapFrom(src => src.prevInterimRestrictionsOrderStartDate))
            .ForMember(dest => dest.PrevInterimRestrictionsOrderEndDate, opt => opt.MapFrom(src => src.prevInterimRestrictionsOrderEndDate))
            .ForMember(dest => dest.PractitionerName, opt => opt.MapFrom(src => src.insolvencyPractitionerName))
            .ForMember(dest => dest.PractitionerFirmName, opt => opt.MapFrom(src => src.insolvencyPractitionerFirmName))
            .ForMember(dest => dest.PractitionerAddress, opt => opt.MapFrom(src => src.insolvencyPractitionerAddress))
            .ForMember(dest => dest.PractitionerPostcode, opt => opt.MapFrom(src => src.insolvencyPractitionerPostcode))
            .ForMember(dest => dest.PractitionerTelephone, opt => opt.MapFrom(src => src.insolvencyPractitionerTelephone))
            .ForMember(dest => dest.InsolvencyServiceOffice, opt => opt.MapFrom(src => src.insolvencyServiceOffice))
            .ForMember(dest => dest.InsolvencyServiceContact, opt => opt.MapFrom(src => src.insolvencyServiceContact))
            .ForMember(dest => dest.InsolvencyServiceAddress, opt => opt.MapFrom(src => src.insolvencyServiceAddress))
            .ForMember(dest => dest.InsolvencyServicePostcode, opt => opt.MapFrom(src => src.insolvencyServicePostcode))
            .ForMember(dest => dest.InsolvencyServiceTelephone, opt => opt.MapFrom(src => src.insolvencyServicePhone))
            .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(src => src.caseNo))
            .ForMember(dest => dest.IndividualNumber, opt => opt.MapFrom(src => src.indivNo))
            .ForMember(dest => dest.DateOfPreviousOrder, opt => opt.MapFrom(src => src.dateOfPreviousOrder))
            .ForMember(dest => dest.DeceasedDate, opt => opt.MapFrom(src => src.deceasedDate))
            .ReverseMap()
            .ForPath(s => s.Trading,
                    opt => opt.MapFrom(
                        src => !string.IsNullOrEmpty(src.TradingData) && src.TradingData != "<No Trading Names Found>"
                            ? src.TradingData.ParseXml<Trading>() : null));            
        }       
    }

    public static class TradingExtensions
    {
        public static T ParseXml<T>(this string value) where T : class
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var textReader = new StringReader(value))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }
    }
}
