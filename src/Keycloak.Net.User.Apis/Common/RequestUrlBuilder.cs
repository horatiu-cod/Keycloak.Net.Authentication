using Keycloak.Net.Authentication;
using Microsoft.Extensions.Options;
using System.Text;

namespace Keycloak.Net.User.Apis.Common;

internal class RequestUrlBuilder : IRequestUrlBuilder
{
    private readonly IOptions<JwtBearerValidationOptions> _options;

    public RequestUrlBuilder(IOptions<JwtBearerValidationOptions> options)
    {
        _options = options;
    }

    string authority => _options.Value.Authority!;

    private const string adminApi = "admin/realms";
    public string AdminApi
    {
        get
        {
            StringBuilder sb = new StringBuilder(authority);
            sb.Replace("realms", adminApi);
            return sb.ToString();
        }
    }
    public string TokenApi
    {
        get
        {
            var url = authority.EndsWith('/') ? authority.TrimEnd('/') : authority;
            var sb = new StringBuilder(url);
            sb.Append("/protocol/openid-connect/token");
            return sb.ToString();
        }
    }
    public string LogoutApi
    {
        get
        {
            var url = authority.EndsWith('/') ? authority.TrimEnd('/') : authority;
            var sb = new StringBuilder(url);
            sb.Append("/protocol/openid-connect/logout");
            return sb.ToString();
        }
    }
}
