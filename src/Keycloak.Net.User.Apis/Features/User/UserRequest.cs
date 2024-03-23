using Keycloak.Net.User.Apis.Common;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User;

internal class UserRequest : IUserRequest
{
    private readonly IRequestUrlBuilder? _url;

    public UserRequest() { }
    public UserRequest(IRequestUrlBuilder url) => _url = url;

    public async Task<Result<UserRequestDto>> GetUserAsync(string username, string accessToken, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        try
        {
            var result = await httpClient.GetAsync($"{_url.AdminApi}/users/?username={username}", cancellationToken);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<UserRequestDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetUserAsync");
            }
            else if (!result.IsSuccessStatusCode)
            {
                return Result<UserRequestDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetUserAsync");
            }
            else if (result.StatusCode != HttpStatusCode.OK)
            {
                return Result<UserRequestDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetUserAsync");
            }
            else
            {
                var userDtos = await result.Content.ReadFromJsonAsync<IEnumerable<UserRequestDto>>();
                var userDto = userDtos!.Where(n => n.UserName == username).FirstOrDefault();
                return Result<UserRequestDto>.Success(userDto, result.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result<UserRequestDto>.Fail($"{ex.Message} from GetUserAsync");
        }
    }
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
            return Result<string>.Fail($"{ex.Message} from GetUserAsync");
        }
    }

}
