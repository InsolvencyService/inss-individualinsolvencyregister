using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.SearchModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace INSS.EIIR.DataAccess;

public class IndividualRepository : IIndividualRepository
{
    private readonly EIIRContext _context;

    public IndividualRepository(EIIRContext context)
    {
        _context = context;
    }

    public IEnumerable<CaseResult> BuildEiirIndex()
    {
        List<CaseResult> results;

        using (_context)
        {
            _context.Database.SetCommandTimeout(600);

            results = _context.CaseResults
                .FromSqlRaw("exec getEiirIndex")
                .ToList();
        }

        return results;
    }
}