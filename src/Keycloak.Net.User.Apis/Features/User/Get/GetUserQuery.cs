namespace Keycloak.Net.User.Apis.Features.User.Get;

internal class GetUserQuery(IGetUserQueryInternal getUserQueryInternal, IGetClientTokenQuery getClientTokenQuery, IHttpClientFactory httpClientFactory, IOptionsMonitor<Server> server, IOptionsMonitor<AdminClient> adminClient) : IGetUserQuery
{
    private readonly IGetUserQueryInternal _getUserQueryInternal = getUserQueryInternal;
    private readonly IGetClientTokenQuery _getClientTokenQuery = getClientTokenQuery;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Server _server = server.CurrentValue;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;

    public async Task<Result<GetUserResponse?>> Handler( string username, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        string url = BaseUrl.AdminUrl(_server.BaseAddress, _server.RealmName);

        var client = new GetClientTokenRequest(_adminClient.ClientId, _adminClient.ClientSecret);
        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess)
            return Result<GetUserResponse>.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content!.AccessToken!);

        var response = await _getUserQueryInternal.Handler(url, username, httpClient, cancellationToken);
        if (!response.IsSuccess)
            return Result<GetUserResponse?>.Fail(response.StatusCode, response.Error);
        return Result<GetUserResponse?>.Success(response.Content, response.StatusCode);
    }
}
