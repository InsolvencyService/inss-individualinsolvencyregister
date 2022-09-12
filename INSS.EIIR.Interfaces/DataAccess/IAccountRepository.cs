namespace INSS.EIIR.Interfaces.DataAccess;

public interface IAccountRepository
{
    bool AdminAccountIsValid(string username, string password);
}