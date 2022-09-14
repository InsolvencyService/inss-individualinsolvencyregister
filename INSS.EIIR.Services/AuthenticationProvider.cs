using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;
using INSS.EIIR.Models.Authentication;

namespace INSS.EIIR.Services;

public class AuthenticationProvider : IAuthenticationProvider
{
    private readonly IAccountRepository _accountRepository;

    public AuthenticationProvider(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public User GetAdminUser(string username, string password)
    {
        return _accountRepository.GetAdminUser(username, password);
    }

    public User GetSubscriberUser(string username, string password)
    {
        return _accountRepository.GetSubscriberUser(username, password);
    }
}