using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.DeleteUser
{
    internal interface IDeleteUserCommand
    {
        Task<Result> DeleteUserAsync(string baseAddress, string realmName, string userId, string realmAdminClientId, string realmAdminClientSecret, CancellationToken cancellationToken);
    }
}