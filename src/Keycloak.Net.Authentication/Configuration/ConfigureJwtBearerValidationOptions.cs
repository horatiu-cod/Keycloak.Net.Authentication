﻿using Keycloak.Net.Authentication.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Keycloak.Net.Authentication.Configuration;

public class ConfigureJwtBearerValidationOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private JwtBearerValidationOptions JwtBearerValidationOptions { get; }

    public ConfigureJwtBearerValidationOptions(IOptionsMonitor<JwtBearerValidationOptions> jwtBearerValidationOptions)
    {
        JwtBearerValidationOptions = jwtBearerValidationOptions.CurrentValue;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        //if (string.Equals(name, JwtBearerDefaults.AuthenticationScheme))
        //{
        //    Configure(options);
        //}
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
