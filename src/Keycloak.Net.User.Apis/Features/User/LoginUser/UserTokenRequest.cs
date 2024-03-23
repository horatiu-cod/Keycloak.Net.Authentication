using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;
using System.Net.Http.Json;
using System.Net;

namespace Keycloak.Net.User.Apis.Features.User.LoginUser;

internal class UserTokenRequest
{
    private readonly IRequestUrlBuilder _url;

    public UserTokenRequest(IRequestUrlBuilder url) => _url = url;

    public async Task<Result<TokenResponseDto?>> LoginUserAsync(UserTokenRequestDto userRequestDto, HttpClient httpClient, CancellationToken cancellationToken)
    {
        var requestBody = UserTokenRequestBodyBuilder.UserTokenRequestBody(userRequestDto);
		try
		{
            var response = await httpClient.PostAsync(_url.TokenApi, requestBody, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<TokenResponseDto>.Fail(response.StatusCode, $"{response.StatusCode} from LoginUserAsync");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result<TokenResponseDto>.Fail(response.StatusCode, $"{response.StatusCode} from LoginUserAsync");
            }
            else
            {
                var keycloakTokenResponseDto = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
                return Result<TokenResponseDto?>.Success( keycloakTokenResponseDto, response.StatusCode);
            }
        }
        catch (Exception ex)
		{
            return Result<TokenResponseDto>.Fail($"{ ex.Message} from LoginUserAsync");
        }
    }
}
