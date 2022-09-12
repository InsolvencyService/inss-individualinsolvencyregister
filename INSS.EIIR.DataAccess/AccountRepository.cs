using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;

namespace INSS.EIIR.DataAccess;

public class AccountRepository : IAccountRepository
{
    private readonly EIIRContext _context;

    public AccountRepository(EIIRContext context)
    {
        _context = context;
    }

    public bool AdminAccountIsValid(string username, string password)
    {
        using (_context)
        {
            var adminAccount = _context.SecretariatAccounts
                .SingleOrDefault(sa => sa.AccountActive == "Y" && sa.SecretariatLogin == username && sa.SecretariatPassword == password);

            return adminAccount != null;
        }
    }
}