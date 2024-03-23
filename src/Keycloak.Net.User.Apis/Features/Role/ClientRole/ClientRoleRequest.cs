using Keycloak.Net.User.Apis.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;

namespace Keycloak.Net.User.Apis.Features.Role.ClientRole;

internal class ClientRoleRequest
{
    private readonly IRequestUrlBuilder _url;

    public ClientRoleRequest(IRequestUrlBuilder url) => _url = url;

    public async Task<Result<RoleDto>> GetClientRoleAsync(string accessToken, string clientUuid, string roleName, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		try
		{
            var result = await httpClient.GetAsync($"{_url.AdminApi}/clients/{clientUuid}/roles/{roleName}", cancellationToken);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<RoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetClientRoleAsync");
            }
            else if (!result.IsSuccessStatusCode)
            {
                return Result<RoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetClientRoleAsync");
            }
            else if (result.StatusCode != HttpStatusCode.OK)
            {
                return Result<RoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetClientRoleAsync");
            }
            else
            {
                var roleDto = await result.Content.ReadFromJsonAsync<RoleDto>(cancellationToken);
                return Result<RoleDto>.Success(roleDto, result.StatusCode);
            }
        }
        catch (Exception ex)
		{
            return Result<RoleDto>.Fail( $"{(int)HttpStatusCode.InternalServerError}: {ex.Message} exception from GetClientRoleAsync");
        }
    }
}
