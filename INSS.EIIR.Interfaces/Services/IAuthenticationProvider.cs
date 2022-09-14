using INSS.EIIR.Models.Authentication;

namespace INSS.EIIR.Interfaces.Services;

public interface IAuthenticationProvider
{
    User GetAdminUser(string username, string password);

    User GetSubscriberUser(string username, string password);
}