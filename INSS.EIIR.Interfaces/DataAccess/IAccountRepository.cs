using INSS.EIIR.Models.Authentication;

namespace INSS.EIIR.Interfaces.DataAccess;

public interface IAccountRepository
{
    User GetAdminUser(string username, string password);

    User GetSubscriberUser(string username, string password);
}