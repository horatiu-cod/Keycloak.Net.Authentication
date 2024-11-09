using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Role.ClientRole;

internal class ClientRoleRequest : IClientRoleRequest
{
    public async Task<Result<string?>> GetClientRoleAsync(string url, string accessToken, string clientUuid, string roleName, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        try
        {
            var result = await httpClient.GetAsync($"{url}/clients/{clientUuid}/roles/{roleName}", cancellationToken);

            if (!result.IsSuccessStatusCode && result.StatusCode != HttpStatusCode.OK)
            {
                return Result<string>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetClientRoleAsync");
            }
            else
            {
                var roleDto = await result.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                var uuid = roleDto?["uuid"]?.ToString();
                return Result<string?>.Success(uuid, result.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(HttpStatusCode.InternalServerError, $"{(int)HttpStatusCode.InternalServerError}: {ex.Message} exception from GetClientRoleAsync");
        }
    }
}
