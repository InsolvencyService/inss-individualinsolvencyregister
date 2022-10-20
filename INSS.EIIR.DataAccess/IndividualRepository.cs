using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
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

    public IEnumerable<SearchResult> SearchByName(string firstName = "", string lastName = "")
    {
        List<SearchResult> results;

        using (_context)
        {
            var surname = new SqlParameter("@Surname", lastName);
            var forename = new SqlParameter("@Forename", firstName);

            _context.Database.SetCommandTimeout(600);

            results = _context.SearchResults
                .FromSqlRaw("exec getIndividualByName @Surname, @Forename", surname, forename)
                .ToList();
        }

        return results;
    }
}