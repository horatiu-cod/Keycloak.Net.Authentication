using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;
using System.Net;
using System.Net.Http.Json;

namespace Keycloak.Net.User.Apis.Features.User.LogoutUser;

internal class LogoutUserRequest
{
    private readonly IRequestUrlBuilder _url;

    public LogoutUserRequest(IRequestUrlBuilder url) => _url = url;

    public async Task<Result> LogoutUserAsync(LogoutUserRequestDto logoutUserRequestDto, HttpClient httpClient, CancellationToken cancellationToken)
    {
        var requestBody = LogoutUserRequestBodyBuilder.LogoutUserTokenRequestBody(logoutUserRequestDto);
        try
		{
            var response = await httpClient.PostAsync(_url.TokenApi, requestBody, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result.Fail(response.StatusCode, $"{response.StatusCode} from LogoutUserAsync");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result.Fail(response.StatusCode, $"{response.StatusCode} from LogoutUserAsync");
            }
            else
            {
                var keycloakTokenResponseDto = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
                return Result.Success(response.StatusCode);
            }
        }
        catch (Exception ex)
		{
            return Result.Fail($"{ex.Message} from LogoutUserAsync");
        }
    }
}
