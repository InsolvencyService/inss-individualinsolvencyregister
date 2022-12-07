using AutoMapper;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.SearchModels;

namespace INSS.EIIR.Models.AutoMapperProfiles;

public class IndividualSearchMapper : Profile
{
    public IndividualSearchMapper()
    {
        CreateMap<IndividualSearch, CaseResult>()
            .ForMember(m => m.individualForenames, opt => opt.MapFrom(s => s.FirstName))
            .ForMember(m => m.individualSurname, opt => opt.MapFrom(s => s.FamilyName))
            .ForMember(m => m.individualTitle, opt => opt.MapFrom(s => s.Title))
            .ForMember(m => m.individualAlias, opt => opt.MapFrom(s => s.AlternativeNames))

            .ForMember(m => m.individualGender, opt => opt.MapFrom(s => s.Gender))
            .ForMember(m => m.individualDOB, opt => opt.MapFrom(s => s.DateOfBirth))
            .ForMember(m => m.individualOccupation, opt => opt.MapFrom(s => s.Occupation))
            .ForMember(m => m.individualTown, opt => opt.MapFrom(s => s.LastKnownTown))
            .ForMember(m => m.individualAddress, opt => opt.MapFrom(s => s.LastKnownAddress))
            .ForMember(m => m.individualPostcode, opt => opt.MapFrom(s => s.LastKnownPostcode))
            .ForMember(m => m.individualAddressWithheld, opt => opt.MapFrom(s => s.AddressWithheld))
            .ForMember(m => m.caseName, opt => opt.MapFrom(s => s.CaseName))
            .ForMember(m => m.courtName, opt => opt.MapFrom(s => s.Court))
            .ForMember(m => m.courtNumber, opt => opt.MapFrom(s => s.CourtNumber))
            .ForMember(m => m.caseYear, opt => opt.MapFrom(s => s.CaseYear))
            .ForMember(m => m.insolvencyType, opt => opt.MapFrom(s => s.InsolvencyType))
            .ForMember(m => m.notificationDate, opt => opt.MapFrom(s => s.NotificationDate))
            .ForMember(m => m.caseStatus, opt => opt.MapFrom(s => s.CaseStatus))
            .ForMember(m => m.insolvencyDate, opt => opt.MapFrom(s => s.InsolvencyDate))
            .ForMember(m => m.insolvencyPractitionerName, opt => opt.MapFrom(s => s.PractitionerName))
            .ForMember(m => m.insolvencyPractitionerFirmName, opt => opt.MapFrom(s => s.PractitionerFirmName))
            .ForMember(m => m.insolvencyPractitionerAddress, opt => opt.MapFrom(s => s.PractitionerAddress))
            .ForMember(m => m.insolvencyPractitionerPostcode, opt => opt.MapFrom(s => s.PractitionerPostcode))
            .ForMember(m => m.insolvencyPractitionerTelephone, opt => opt.MapFrom(s => s.PractitionerTelephone))
            .ForMember(m => m.insolvencyServiceOffice, opt => opt.MapFrom(s => s.InsolvencyServiceOffice))
            .ForMember(m => m.insolvencyServiceContact, opt => opt.MapFrom(s => s.InsolvencyServiceContact))
            .ForMember(m => m.insolvencyServiceAddress, opt => opt.MapFrom(s => s.InsolvencyServiceAddress))
            .ForMember(m => m.insolvencyServicePostcode, opt => opt.MapFrom(s => s.InsolvencyServicePostcode))
            .ForMember(m => m.insolvencyServicePhone, opt => opt.MapFrom(s => s.InsolvencyServiceTelephone))
            .ForMember(m => m.tradingNames, opt => opt.MapFrom(s => s.InsolvencyTradeName))
            .ForMember(m => m.caseDescription, opt => opt.MapFrom(s => s.CaseDescription))
            .ForMember(m => m.caseNo, opt => opt.MapFrom(s => s.CaseNumber))
            .ForMember(m => m.indivNo, opt => opt.MapFrom(s => s.IndividualNumber)).ReverseMap();


        CreateMap<IndividualSearch, SearchResult>()
            .ForMember(m => m.individualForenames, opt => opt.MapFrom(s => s.FirstName))
            .ForMember(m => m.individualSurname, opt => opt.MapFrom(s => s.FamilyName))
            .ForMember(m => m.individualAlias, opt => opt.MapFrom(s => s.AlternativeNames))
            .ForMember(m => m.companyName, opt => opt.MapFrom(s => s.InsolvencyTradeName))
            .ForMember(m => m.individualTown, opt => opt.MapFrom(s => s.LastKnownTown))
            .ForMember(m => m.individualPostcode, opt => opt.MapFrom(s => s.LastKnownPostcode))
            .ForMember(m => m.caseNo, opt => opt.MapFrom(s => s.CaseNumber))
            .ForMember(m => m.indivNo, opt => opt.MapFrom(s => s.IndividualNumber)).ReverseMap();
    }
}