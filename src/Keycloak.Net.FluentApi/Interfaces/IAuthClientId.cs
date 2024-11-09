namespace Keycloak.Net.FluentApi.Interfaces;

public interface IAuthClientId
{
    IAuthClientSecret ClientSecret(string clientSecret);
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
