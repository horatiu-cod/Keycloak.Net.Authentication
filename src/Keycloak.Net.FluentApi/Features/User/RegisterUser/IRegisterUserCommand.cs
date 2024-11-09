using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.User.RegisterUser;

public interface IRegisterUserCommand
{
    Task<Result> RegisterUser(string url, string uri, string clientId, string clientSecret, string username, string password, string email, string firstName, string lastName, CancellationToken cancellationToken = default);
}