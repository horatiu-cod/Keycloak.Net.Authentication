namespace Keycloak.Net.User.Apis.Features.User.LogoutUser;

internal record struct LogoutUserRequestDto
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RefreshToken { get; set; }

}
