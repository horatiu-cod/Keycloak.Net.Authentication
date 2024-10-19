namespace Keycloak.Net.User.Apis.Features.User.Get;

internal class GetUserQueryInternal : IGetUserQueryInternal
{

    public async Task<Result<GetUserResponse?>> Handler(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"{url}/users?username={username}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result<GetUserResponse?>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                var results = await response.Content.ReadFromJsonAsync<GetUserResponse[]>();
                var result = results?.SingleOrDefault(results => results.UserName == username);
                if (result is not null && !string.IsNullOrEmpty(result.Id))
                {
                    return Result<GetUserResponse?>.Success(result, response.StatusCode);
                }
                else
                {
                    return Result<GetUserResponse?>.Fail(HttpStatusCode.NotFound, $"{username} was not found");
                }
            }
        }
        catch (Exception ex)
        {
            return Result<GetUserResponse?>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong /br{ex.Message}");
        }

    }
}
