using INSS.EIIR.Interfaces.AzureSearch;
using INSS.EIIR.Models;
using INSS.EIIR.Models.SearchModels;
using Microsoft.AspNetCore.Mvc;

namespace INSS.EIIR.AzureSearch.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndividualSearchController : ControllerBase
    {
        private readonly ILogger<IndividualSearchController> _logger;
        private readonly IIndividualQueryService _queryService;

        public IndividualSearchController(
            ILogger<IndividualSearchController> logger,
            IIndividualQueryService queryService
            )
        {
            _logger = logger;
            _queryService = queryService;
        }

        [HttpPost("individuals")]
        public async Task<IEnumerable<SearchResult>> PostAsync([FromBody]IndividualSearchModel searchModel)
        {
            return await _queryService.SearchIndexAsync(searchModel);
        }
    }
}