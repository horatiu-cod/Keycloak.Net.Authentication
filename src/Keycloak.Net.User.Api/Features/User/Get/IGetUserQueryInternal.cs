using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Get;

internal interface IGetUserQueryInternal
{
    Task<Result<GetUserResponse?>> Handler(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default);
}