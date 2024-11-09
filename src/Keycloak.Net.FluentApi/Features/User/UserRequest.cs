using Keycloak.Net.FluentApi.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.FluentApi.Features.User;

internal class UserRequest : IUserRequest
{
    public async Task<Result<string?>> GetUserAsync(string username, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        try
        {
            var result = await httpClient.GetAsync($"{url}/users/?username={username}", cancellationToken);

            if (!result.IsSuccessStatusCode)
            {
                return Result<string>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetUserAsync");
            }
            else
            {
                var userDtos = await result.Content.ReadFromJsonAsync<IEnumerable<JsonObject>>();
                var user = userDtos!.FirstOrDefault();
                if (user == null)
                    return Result<string>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetUserAsync");
                var userId = (string?)user["userId"];
                return Result<string?>.Success(userId, result.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(HttpStatusCode.InternalServerError,$"{ex.Message} from GetUserAsync");
        }
    }

}
