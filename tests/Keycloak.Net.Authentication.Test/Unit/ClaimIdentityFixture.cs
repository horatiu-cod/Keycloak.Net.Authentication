using System.Security.Claims;

namespace Keycloak.Net.Authentication.Test.Unit;

public class ClaimIdentityFixture
{
    public ClaimIdentityFixture()
    {
        SetClaimsIdentity = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClientResourceClaimType, ClientResourceClaimValue, MyValueType, MyUrl, MyUrl),
        new Claim(RealmClaimType, RealmClaimValue, MyValueType, MyUrl, MyUrl),
        new Claim("preferred_username", "horatiu"),
        new Claim(ClaimTypes.Name, "horatiu cod"),
        new Claim(ClaimTypes.Email, "horatiu@52.ro"),
        new Claim("iss", MyUrl)
        ]));

    }
    public ClaimsPrincipal? SetClaimsIdentity { get; private set; }

    // The resource_access claim type
    private const string ClientResourceClaimType = "resource_access";
    private const string ClientResourceClaimValue = @$"{{""{MyAudience}"":{{""roles"":[""{FirstClientClaim}"",""{SecondClientClaim}""]}}}}";

    // The realm_access claim type
    private const string RealmClaimType = "realm_access";
    private const string RealmClaimValue = @$"{{""roles"":[""{FirstRealmClaim}"",""{SecondRealmClaim}""]}}";

    // Fake claim values
    public const string FirstClientClaim = "client_first_role";
    private const string SecondClientClaim = "client_second_role";
    private const string FirstRealmClaim = "realm_first_role";
    private const string SecondRealmClaim = "realm_second_role";


    // The issuer/original issuer
    private const string MyUrl = "https://keycloak.mydomain.com/realms/realm";

    // The value type should be JSON
    private const string MyValueType = "JSON";

    // The audience for the resource_access
    private const string MyAudience = "audience";
}
