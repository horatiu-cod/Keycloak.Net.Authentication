using Keycloak.Net.User.Apis.Common;
using System.Net;
using System.Net.Http.Headers;

namespace Keycloak.Net.User.Apis.Features.User.DeleteUser;

internal class DeleteUserRequest
{
    private readonly IRequestUrlBuilder _url;

    public DeleteUserRequest(IRequestUrlBuilder url) => _url = url;

    public async Task<Result> DeleteUserAsync(DeleteUserRequestDto requestDto, string accessToken, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		try
		{
            var result = await httpClient.DeleteAsync($"/{_url.AdminApi}/users/{requestDto.UserId}", cancellationToken);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result.Fail(result.StatusCode, $"{(int)result.StatusCode} from ResetPasswordAsync");
            }
            else if (!result.IsSuccessStatusCode)
            {
                return Result.Fail(result.StatusCode, $"{(int)result.StatusCode} from ResetPasswordAsync");
            }
            else if (result.StatusCode != HttpStatusCode.OK)
            {
                return Result.Fail(result.StatusCode, $"{(int)result.StatusCode} from ResetPasswordAsync");
            }
            else
            {
                return Result.Success(result.StatusCode);
            }
        }
        catch (Exception ex)
		{
            return Result.Fail($"{ex.Message} from ResetPasswordAsync");
        }
    }
}
