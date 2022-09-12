namespace INSS.EIIR.Interfaces.Services;

public interface IAuthenticationProvider
{
    bool AdminAccountIsValid(string username, string password);
}