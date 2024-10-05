using Keycloak.Net.Authentication.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Keycloak.Net.Authentication.Configuration;

public class ConfigureJwtBearerValidationOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private JwtBearerValidationOptions JwtBearerValidationOptions { get; }
    private IHttpContextAccessor _httpContextAccessor { get; }

    public ConfigureJwtBearerValidationOptions(IOptionsMonitor<JwtBearerValidationOptions> jwtBearerValidationOptions, IHttpContextAccessor httpContextAccessor)
    {
        JwtBearerValidationOptions = jwtBearerValidationOptions.CurrentValue;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (string.Equals(name, JwtBearerDefaults.AuthenticationScheme))
        {
            Configure(options);
        }
        else 
        {
            Configure(options);
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        //var context = _httpContextAccessor.HttpContext;
        //var client = context.GetEndpoint()?.Metadata.GetMetadata<ClientAttribute>()?.ClientName ?? null;
        JwtBearerOptionsMapper.MapFromJwtBearerValidationOptions(JwtBearerValidationOptions, options);
    }
}
