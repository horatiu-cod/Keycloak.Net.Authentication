using Keycloak.Net.User.Apis.Common;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User;

internal class GetUserQuery : IGetUserQuery
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
                var results = await response.Content.ReadFromJsonAsync<GetUserResponse[]>() ;
                //var result = results?.FirstOrDefault( results => results.UserName == username);
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
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong /br{ex.Message}");
        }
    }
    public async Task<Result<GetUserResponse?>> GetUserAsync(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default)
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
                var result = results?.FirstOrDefault(results => results.UserName == username);
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
