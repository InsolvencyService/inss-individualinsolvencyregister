using INSS.EIIR.Data.Models;
using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Models.Authentication;
using INSS.EIIR.Models.Constants;

namespace INSS.EIIR.DataAccess;

public class AccountRepository : IAccountRepository
{
    private readonly EIIRContext _context;

    public AccountRepository(EIIRContext context)
    {
        _context = context;
    }

    public User GetAdminUser(string username, string password)
    {
        using (_context)
        {
            var adminAccount = _context.SecretariatAccounts
                .SingleOrDefault(sa => sa.AccountActive == "Y" && sa.SecretariatLogin == username && sa.SecretariatPassword == password);

            return adminAccount != null ? CreateUser(username, password, Role.Admin) : null;
        }
    }

    public User GetSubscriberUser(string username, string password)
    {
        using (_context)
        {
            var subscriberAccount = _context.SubscriberAccounts
                .SingleOrDefault(sa => sa.AccountActive == "Y" && sa.SubscriberLogin == username && sa.SubscriberPassword == password);

            return subscriberAccount != null ? CreateUser(username, password, Role.Subscriber) : null;
        }
    }

    private static User CreateUser(string username, string password, string role)
    {
        return new User
        {
            UserName = username,
            Password = password,
            UserRole = role
        };
    }
}