using Keycloak.Net.Authentication.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Keycloak.Net.Authentication.Configuration;

internal class ConfigureJwtBearerValidationOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private IOptions<JwtBearerValidationOptions> JwtBearerValidationOptions { get; }

    public ConfigureJwtBearerValidationOptions(IOptions<JwtBearerValidationOptions> jwtBearerValidationOptions)
    {
        JwtBearerValidationOptions = jwtBearerValidationOptions;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (string.Equals(name, JwtBearerDefaults.AuthenticationScheme))
        {
            Configure(options);
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        options = JwtBearerValidationOptions.Value.MapFromJwtBearerValidationOptions();

        options.TokenValidationParameters = JwtBearerValidationOptions.Value.MapFromTokenValidationParametersOptions();
    }
}
