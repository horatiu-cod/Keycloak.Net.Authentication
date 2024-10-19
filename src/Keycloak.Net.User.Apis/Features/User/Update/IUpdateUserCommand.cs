
namespace Keycloak.Net.User.Apis.Features.User.Update;

internal interface IUpdateUserCommand
{
    Task<Result> UpdateUserAsync(string username, string? firstName, string? lastName, string? email, CancellationToken cancellationToken = default);
}