namespace Keycloak.Net.FluentApi.Interfaces;

public interface IUserPassword
{
    IUserEmail UserEmail(string email);
}

//public interface IRealmRole
//{
//    Result Register();
//}

//public interface IClientRole
//{
//    IClientId ForClient(string clientId);
//}

//public interface IClientId
//{
//    Result Register();
//}
