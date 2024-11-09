using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Interfaces;

public interface IUserLastName
{
    //IRealmRole RealmRole(params string[] realmRole);
    //IClientRole ClientRole(params string[] clientRole);
    Result Register();
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
