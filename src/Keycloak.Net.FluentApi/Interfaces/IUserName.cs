namespace Keycloak.Net.FluentApi.Interfaces;

public interface IUserName
{
    IUserPassword UserPassword(string password);
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
