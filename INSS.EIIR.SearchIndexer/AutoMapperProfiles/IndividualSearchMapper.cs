using AutoMapper;
using INSS.EIIR.Models;
using INSS.EIIR.SearchIndexer.IndexModels;

namespace INSS.EIIR.SearchIndexer.AutoMapperProfiles;

public class IndividualSearchMapper :Profile
{
    public IndividualSearchMapper()
    {
        CreateMap<SearchResult, IndividualSearch>();
    }
}