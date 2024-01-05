using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
namespace Keycloak.Net.Authentication;

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
        options = Mapper.MapFromJwtBearerValidationOptions(JwtBearerValidationOptions.Value);

        options.TokenValidationParameters = Mapper.MapFromTokenValidationParametersOptions(JwtBearerValidationOptions.Value);
    }
}
