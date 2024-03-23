using Keycloak.Net.User.Apis.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Keycloak.Net.User.Apis.Features.User.ResetPassword;

internal class ResetPasswordRequest
{
    private readonly IRequestUrlBuilder _url;

    public ResetPasswordRequest(IRequestUrlBuilder url) => _url = url;

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequestDto requestDto, string accessToken, string password, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		try
		{
			var response = await httpClient.PutAsJsonAsync<string>($"{_url.AdminApi}/users/{requestDto.UserId}/reset-password", password, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result.Fail(response.StatusCode, $"{(int)response.StatusCode} from ResetPasswordAsync");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result.Fail(response.StatusCode, $"{(int)response.StatusCode} from ResetPasswordAsync");
            }
            else if (response.StatusCode != HttpStatusCode.NoContent)
            {
                return Result.Fail(response.StatusCode, $"{(int)response.StatusCode} from ResetPasswordAsync");
            }
            else
            {
                return Result.Success(response.StatusCode);
            }
        }
        catch (Exception ex)
		{
            return Result.Fail($"{ex.Message} from ResetPasswordAsync");
        }
    }
}
