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
    private readonly DatabaseConfig _databaseConfig;
    private readonly IMapper _mapper;

    public ExtractRepository(
        EIIRExtractContext eiirExtractContext,
        IOptions<DatabaseConfig> options,
        IMapper mapper)
    {
        _context = eiirExtractContext;
        _databaseConfig = options.Value;
        _mapper = mapper;
    }

    public ExtractAvailable GetExtractAvailable()
    {
        string sql = $"EXEC {_databaseConfig.GetExtractAvailableProcedure}";
        var result = _context.ExtractAvailability.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();
        ExtractAvailable extractAvailabe = _mapper.Map<ExtractAvailabilitySP, ExtractAvailable>(result);

        return extractAvailabe;
    }

    public void UpdateExtractAvailable()
    {
        string sql = $"EXEC {_databaseConfig.UpdateExtractAvailableProcedure}";
        _context.Database.ExecuteSqlRaw(sql);
    }
}
