using ConfigurationSubstitution;
using Keycloak.Net.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.EnableSubstitutions();

builder.Services
    .AddKeyCloakAuthentication()
    .AddKeyCloakJwtBearerOptions("appsettings_section_name")


    //.AddKeyCloakJwtBearerOptions("appsettings_section_name", o =>
    //{
    //    o.Authority = "https://localhost:8843/realms/Test";
    //    //o.Audience = "maui-client";
    //    o.SaveToken = true;

    //    o.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(30);
    //})
    //.AddKeyCloakJwtBearerOptions(c =>
    //{
    //    c.Authority = "https://localhost:8843/realms/Test";
    //    c.ValidAudience = "maui-client";
    //})
    //.AddJwtBearerOptions(options =>
    //{
    //    options.Authority = "https://localhost:8843/realms/Test";
    //    //options.Audience = "maui-client";
    //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    //    {
    //        ValidAudiences = ["maui-client"]
    //    };
    //})
    .AddAuthorization(configure =>
    {
        configure.AddPolicy("email_verified", configure =>
        {
            configure.RequireClaim("email_verified", "true");
        });
        configure.AddPolicy("role", policy =>
        {
            policy.RequireClaim("role", "user_role");
        });
    })
    ;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("api/authenticate", () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization();

app.MapGet("api/authorize", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("email_verified");
app.MapGet("api/attribute",[Authorize(Roles = "user_role")] () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"));

app.MapGet("api/role", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("role");

app.Run();
public partial class Program { }