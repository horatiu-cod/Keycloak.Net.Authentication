using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Get;

internal class GetUserIdQuery : IGetUserIdQuery
{
    public async Task<Result<string?>> GetUserIdAsync(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"{url}/users?username={username}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result<string?>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                var results = await response.Content.ReadFromJsonAsync<GetUserResponse[]>(cancellationToken);
                if (results is not null && results.Length != 0)
                {
                    var result = Array.Find(results, element => element.UserName == username);
                    if (result is not null && !string.IsNullOrEmpty(result.Id))
                    {
                        var userId = result.Id;
                        return Result<string?>.Success(userId, response.StatusCode);
                    }
                    else
                    {
                        return Result<string?>.Fail(HttpStatusCode.NotFound, $"{username} was not found");
                    }
                }
                else
                {
                    return Result<string?>.Fail(HttpStatusCode.NotFound, $"{username} was not found");
                }
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong /br{ex.Message}");
        }
    }

}
