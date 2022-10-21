using AutoMapper;
using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Configuration;
using INSS.EIIR.Models.ExtractModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace INSS.EIIR.DataAccess;

public class ExtractRepository : IExtractRepository
{
    private readonly EIIRExtractContext _context;
    private readonly EIIRContext _eiirContext;
    private readonly DatabaseConfig _databaseConfig;
    private readonly IMapper _mapper;

    public ExtractRepository(
        EIIRExtractContext eiirExtractContext,
        EIIRContext eiirContext,
        IOptions<DatabaseConfig> options,
        IMapper mapper)
    {
        _context = eiirExtractContext;
        _eiirContext = eiirContext;
        _databaseConfig = options.Value;
        _mapper = mapper;
    }

    public Extract GetExtractAvailable()
    {
        string sql = $"EXEC {_databaseConfig.GetExtractAvailableProcedure}";
        var result = _context.ExtractAvailability.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();
        Extract extractAvailabe = _mapper.Map<ExtractAvailabilitySP, Extract>(result);

        return extractAvailabe;
    }

    public void UpdateExtractAvailable()
    {
        string sql = $"EXEC {_databaseConfig.UpdateExtractAvailableProcedure}";
        _context.Database.ExecuteSqlRaw(sql);
    }

    public async Task<IEnumerable<Extract>> GetExtractsAsync(PagingParameters pagingParameters)
    {
        var results = await _eiirContext.ExtractAvailabilities.OrderByDescending(x => x.ExtractId)
                                                              .Skip(pagingParameters.Skip)
                                                              .Take(pagingParameters.PageSize)
                                                              .ToListAsync();

        var extracts = _mapper.Map<IList<ExtractAvailability>, IList<Extract>>(results);

        return extracts;
    }

    public async Task<int> GetTotalExtractsAsync()
    {
        var totalExtracts = await _eiirContext.ExtractAvailabilities.CountAsync();

        return totalExtracts;
    }

}
