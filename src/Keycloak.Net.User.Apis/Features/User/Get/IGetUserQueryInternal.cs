namespace Keycloak.Net.User.Apis.Features.User.Get;

internal interface IGetUserQueryInternal
{
    Task<Result<GetUserResponse?>> Handler(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default);
}