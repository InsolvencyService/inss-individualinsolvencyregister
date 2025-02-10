using AutoMapper;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Helpers;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SearchModels;
using INSS.EIIR.Models;
using Azure.Search.Documents.Indexes.Models;
using System.Globalization;

namespace INSS.EIIR.Models.AutoMapperProfiles;

public class IndividualSearchMapper : Profile
{
    public IndividualSearchMapper()
    {
        CreateMap<CaseResult, IndividualSearch>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => IirEncodingHelper.FixSQLEncoding(src.individualForenames).ToUpper()))
            .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => IirEncodingHelper.FixSQLEncoding(src.individualSurname).ToUpper()))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.individualTitle))
            .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.individualAlias))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.individualGender))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.individualDOB.Trim()))
            .ForMember(dest => dest.Occupation, opt => opt.MapFrom(src => src.individualOccupation))
            .ForMember(dest => dest.LastKnownTown, opt => opt.MapFrom(src => src.individualTown))
            .ForMember(dest => dest.LastKnownAddress, opt => opt.MapFrom(src => src.individualAddress))
            .ForMember(dest => dest.LastKnownPostcode, opt => opt.MapFrom(src => src.individualPostcode))
            .ForMember(dest => dest.AddressWithheld, opt => opt.MapFrom(src => src.individualAddressWithheld))
            .ForMember(dest => dest.CaseName, opt => opt.MapFrom(src => IirEncodingHelper.FixSQLEncoding(src.caseName)))
            .ForMember(dest => dest.Court, opt => opt.MapFrom(src => src.courtName))
            .ForMember(dest => dest.CourtNumber, opt => opt.MapFrom(src => src.courtNumber))
            .ForMember(dest => dest.CaseYear, opt => opt.MapFrom(src => src.caseYear))
            .ForMember(dest => dest.InsolvencyType, opt => opt.MapFrom(src => src.insolvencyType))
            .ForMember(dest => dest.NotificationDate, opt => opt.MapFrom(src => src.notificationDate))
            .ForMember(dest => dest.CaseStatus, opt => opt.MapFrom(src => src.caseStatus))
            .ForMember(dest => dest.CaseDescription, opt => opt.MapFrom(src => IirEncodingHelper.FixSQLEncoding(src.caseDescription)))
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
        .ForPath(s => s.individualDOB, opt => opt.MapFrom(src => src.DateOfBirth.Trim()))
        .ForPath(s => s.individualForenames, opt => opt.MapFrom(src => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(src.FirstName.ToLower())))
        .ForPath(s => s.individualSurname, opt => opt.MapFrom(src => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(src.FamilyName.ToLower())))
        .ForPath(s => s.caseName, opt => opt.MapFrom(src => src.CaseName))
        .ForPath(s => s.caseDescription, opt => opt.MapFrom(src => src.CaseDescription));

        CreateMap<IndividualSearch, SearchResult>()
            .ForMember(m => m.individualForenames, opt => opt.MapFrom(s => s.FirstName))
            .ForMember(m => m.individualSurname, opt => opt.MapFrom(s => s.FamilyName))
            .ForMember(m => m.individualAlias, opt => opt.MapFrom(s => s.AlternativeNames))
            .ForMember(m => m.dateOfBirth, opt => opt.MapFrom(s => s.DateOfBirth))
            .ForMember(m => m.companyName, opt => opt.MapFrom(s => s.TradingData))
            .ForMember(m => m.individualTown, opt => opt.MapFrom(s => s.LastKnownTown))
            .ForMember(m => m.individualPostcode, opt => opt.MapFrom(s => s.LastKnownPostcode))
            .ForMember(m => m.caseNo, opt => opt.MapFrom(s => s.CaseNumber))
            .ForMember(m => m.indivNo, opt => opt.MapFrom(s => s.IndividualNumber)).ReverseMap();
    }
}