using Keycloak.Net.FluentApi.Common;
using Keycloak.Net.FluentApi.Features.User.RegisterUser;
using Keycloak.Net.FluentApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.FluentApi;
public sealed class RegisterUser : IRegisterUser
{
    private readonly string _url;
    private readonly string _uri;
    private string _clientId = string.Empty;
    private string _clientSecret = string.Empty;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _email = string.Empty;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private readonly IRegisterUserCommand _request;
    private CancellationToken _cancellationToken;
    private const string adminApi = "admin/realms";

    private RegisterUser(string url, CancellationToken cancellationToken = default)
    {
        IServiceCollection services = new ServiceCollection();
        var scope = services.BuildServiceProvider().CreateScope();
        _uri = url;
        var sb = new StringBuilder(url);
        _url = sb.Replace("realms", adminApi).ToString();
        _request = scope.ServiceProvider.GetRequiredService<IRegisterUserCommand>();
        _cancellationToken = cancellationToken;
    }
    public static IServerUrl RealmUrl(string url, CancellationToken cancellationToken = default) => new RegisterUser(url, cancellationToken);

    public IAuthClientId ClientId(string clientId)
    {
        _clientId = clientId;
        return this;
    }

    public IAuthClientSecret ClientSecret(string clientSecret)
    {
        _clientSecret = clientSecret;
        return this;
    }

    public IUserName UserName(string username)
    {
        _username = username;
        return this;
    }

    public IUserPassword UserPassword(string password)
    {
        _password = password;
        return this;
    }
    public IUserEmail UserEmail(string email)
    {
        _email = email;
        return this;
    }

    public IUserFirstName UserFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public IUserLastName UserLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }
    //public IRealmRole RealmRole(params string[] realmRole)
    //{
    //    _realmRoles = realmRole;
    //    return this;
    //}

    //public IClientRole ClientRole(params string[] clientRole)
    //{
    //    _clientRoles = clientRole;
    //    return this;
    //}

    //public IClientId ForClient(string clientId)
    //{
    //    _clientId = clientId;
    //    return this;
    //}

    public Result Register()
    {
     
        var response = _request.RegisterUser(_url, _uri, _clientId, _clientSecret, _username, _password, _email, _firstName, _lastName, _cancellationToken);
        return response.Result;

    }
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
