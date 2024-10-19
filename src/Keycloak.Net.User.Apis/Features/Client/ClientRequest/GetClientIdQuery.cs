namespace Keycloak.Net.User.Apis.Features.Client.ClientRequest;

internal class GetClientIdQuery : IGetClientIdQuery
{
    public async Task<Result<GetClientIdResponse?>> GetClientIdAsync(string url, string ClientId, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"{url}/clients?clientId={ClientId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result<GetClientIdResponse?>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            var results = await response.Content.ReadFromJsonAsync<GetClientIdResponse[]>(cancellationToken);
            var result = results?.FirstOrDefault();
            if (results is not null && results.Length != 0 && result is not null)
            {
                return Result<GetClientIdResponse?>.Success(result, response.StatusCode);
            }
            else
            {
                return Result<GetClientIdResponse?>.Fail(HttpStatusCode.NotFound, $"{ClientId} was not found");
            }
        }
        catch (Exception ex)
        {
            return Result<GetClientIdResponse?>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
            throw;
        }
    }
}
