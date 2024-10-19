using Keycloak.Net.FluentApi.Common;
using Keycloak.Net.FluentApi.Features.User.RegisterUser;

namespace Keycloak.Net.FluentApi;
public sealed class RegisterUser : IUserName, IUserPassword, IAuthServerUrl, IAuthClientId, IAuthClientSecret, IClientId, IRealmRole, IClientRole
{
    private string? _url;
    private string? _uri;
    private string? _authClientId;
    private string? _authClientSecret;
    private string? _username;
    private string? _password;
    private string[]? _realmRoles;
    private string[]? _clientRoles;
    private string? _clientId;
    private readonly IRegisterUserRequest _request;
    private CancellationToken _cancellationToken;

    private const string adminApi = "admin/realms";

    private RegisterUser(string url)
    {
        _uri = url;
        var sb = new StringBuilder(url);
        _url = sb.Replace("realms", adminApi).ToString();
        _request = new RegisterUserRequest();
        _cancellationToken = new CancellationToken();
    }
    public static IAuthServerUrl Realm(string url) => new RegisterUser(url);

    public IAuthClientId AuthClientId(string authClientId)
    {
        _authClientId = authClientId;
        return this;
    }

    public IAuthClientSecret AuthClientSecret(string authClientSecret)
    {
        _authClientSecret = authClientSecret;
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

    public IRealmRole RealmRole(params string[] realmRole)
    {
        _realmRoles = realmRole;
        return this;
    }

    public IClientRole ClientRole(params string[] clientRole)
    {
        _clientRoles = clientRole;
        return this;
    }

    public IClientId ForClient(string clientId)
    {
        _clientId = clientId;
        return this;
    }

    public Result Register()
    {
        using var httpClient = new HttpClient();
        if (_realmRoles is not null && _realmRoles.Any())
        {
            var res = _request.RegisterUserWithRealmRole(_url, _uri, _authClientId, _authClientSecret, _username, _password, _realmRoles, httpClient, _cancellationToken);
            return res.Result;
        }
        else if (_clientRoles is not null && _clientRoles.Any())
        {
            var res = _request.RegisterUserWithClientRole(_url, _uri ,_authClientId, _authClientSecret, _username, _password, _realmRoles, _clientId, httpClient, _cancellationToken);
            return res.Result;
        }
        else
        {
            var res = _request.RegisterUserWithoutRole(_url, _uri ,_authClientId, _authClientSecret, _username, _password, httpClient, _cancellationToken);
            return res.Result;

        }
    }
}
public interface IAuthServerUrl
{
    IAuthClientId AuthClientId(string authClientId);
}

public interface IAuthClientId
{
    IAuthClientSecret AuthClientSecret(string authClientSecret);
}

public interface IAuthClientSecret
{
    IUserName UserName(string username);
}

public interface IUserName
{
    IUserPassword UserPassword(string password);
}

public interface IUserPassword
{
    IRealmRole RealmRole(params string[] realmRole);
    IClientRole ClientRole(params string[] clientRole);
    Result Register();
}

public interface IRealmRole
{
    Result Register();
}

public interface IClientRole
{
    IClientId ForClient(string clientId);
}

public interface IClientId
{
    Result Register();
}
