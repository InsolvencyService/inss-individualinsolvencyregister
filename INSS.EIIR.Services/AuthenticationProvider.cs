using INSS.EIIR.Interfaces.DataAccess;
using INSS.EIIR.Interfaces.Services;

namespace INSS.EIIR.Services;

public class AuthenticationProvider : IAuthenticationProvider
{
    private readonly IAccountRepository _accountRepository;

    public AuthenticationProvider(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public bool AdminAccountIsValid(string username, string password)
    {
        return _accountRepository.AdminAccountIsValid(username, password);
    }
}