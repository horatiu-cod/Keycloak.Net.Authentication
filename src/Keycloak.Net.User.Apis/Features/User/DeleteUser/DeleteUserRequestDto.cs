namespace Keycloak.Net.User.Apis.Features.User.DeleteUser;

internal record struct DeleteUserRequestDto
{
    public string UserId { get; set; }
}
