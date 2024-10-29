using Keycloak.Net.Authentication.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Keycloak.Net.Authentication.Configuration;

public class ConfigureJwtBearerValidationOptions(IOptionsMonitor<JwtBearerValidationOptions> jwtBearerValidationOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    private JwtBearerValidationOptions JwtBearerValidationOptions { get; } = jwtBearerValidationOptions.CurrentValue;

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (string.Equals(name, "keycloak"))
        {
            Configure(options);
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        JwtBearerOptionsMapper.MapFromJwtBearerValidationOptions(JwtBearerValidationOptions, options);
    }
}
