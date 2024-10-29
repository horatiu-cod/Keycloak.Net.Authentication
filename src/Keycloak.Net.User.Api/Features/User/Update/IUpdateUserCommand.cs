using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Update;

internal interface IUpdateUserCommand
{
    Task<Result> UpdateUserAsync(string username, string? firstName, string? lastName, string? email, CancellationToken cancellationToken = default);
}