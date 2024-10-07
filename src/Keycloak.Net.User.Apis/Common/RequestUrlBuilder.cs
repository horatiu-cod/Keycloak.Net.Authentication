using Keycloak.Net.Authentication;
using Microsoft.Extensions.Options;
using System.Text;

namespace Keycloak.Net.User.Apis.Common;

internal class RequestUrlBuilder : IRequestUrlBuilder
{
    private readonly JwtBearerValidationOptions _options;

    public RequestUrlBuilder(IOptionsMonitor<JwtBearerValidationOptions> options)
    {
        _options = options.CurrentValue;
    }

    private const string adminApi = "admin/realms";

    string BaseUrl
    {
        get
        {
            if (_options.Authority is not null)
                return _options.Authority;
            else if (_options.ValidIssuer is not null)
                return _options.ValidIssuer;
            else return string.Empty;
        }
    }

    public string AdminApi
    {
        get
        {
            StringBuilder sb = new StringBuilder(BaseUrl);
            sb.Replace("realms", adminApi);
            return sb.ToString();
        }
    }
    public string TokenApi
    {
        get
        {
            var url = BaseUrl.EndsWith('/') ? BaseUrl.TrimEnd('/') : BaseUrl;
            var sb = new StringBuilder(url);
            sb.Append("/protocol/openid-connect/token");
            return sb.ToString();
        }
    }
    public string LogoutApi
    {
        get
        {
            var url = BaseUrl.EndsWith('/') ? BaseUrl.TrimEnd('/') : BaseUrl;
            var sb = new StringBuilder(url);
            sb.Append("/protocol/openid-connect/logout");
            return sb.ToString();
        }
    }
}
