using Keycloak.Net.Authentication;
using Microsoft.Extensions.Options;
using System.Text;

namespace Keycloak.Net.User.Apis.Common;

internal class RequestUrlBuilder : IRequestUrlBuilder
{
    private readonly JwtBearerValidationOptions _options;
    private readonly string? _baseAddress;

    public RequestUrlBuilder(IOptionsMonitor<JwtBearerValidationOptions> options, string? baseAddress = default)
    {
        _options = options.CurrentValue;
        _baseAddress = baseAddress;
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
            else return _baseAddress ?? string.Empty;
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
