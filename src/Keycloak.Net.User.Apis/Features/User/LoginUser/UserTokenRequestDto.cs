namespace Keycloak.Net.User.Apis.Features.User.LoginUser;

internal record struct UserTokenRequestDto
{
    public string GrantType { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string UserName { get; set; } 
    public string Password { get; set; }
}
