using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.UpdateUser
{
    internal interface IUpdateUserCommand
    {
        Task<Result> UpdateUserAsync(string baseAddress, string realmName, string clientId, string clientSecret, string username, string? firstName, string? lastName, string? email, CancellationToken cancellationToken = default);
    }
}