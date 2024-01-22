using ConfigurationSubstitution;
using Keycloak.Net.Authentication;
using Keycloak.Net.Authorization;
using Keycloak.Net.Authorization.PermissionAccess;
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
    .AddKeyCloakJwtBearerOptions("Appsettings_section_name")


    //.AddKeyCloakJwtBearerOptions("Appsettings_section_name", o =>
    //{
    //    o.Authority = "https://localhost:8843/realms/Test";
    //    //o.Audience = "client-role";
    //    o.SaveToken = true;

    //    o.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(30);
    //})
    //.AddKeyCloakJwtBearerOptions(c =>
    //{
    //    c.Authority = "https://localhost:8843/realms/Test";
    //    c.ValidAudience = "client-role";
    //})
    //.AddJwtBearerOptions(options =>
    //{
    //    options.Authority = "https://localhost:8843/realms/Test";
    //    //options.Audience = "client-role";
    //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    //    {
    //        ValidAudiences = ["client-role"]
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
            policy.RequireClaim("role", "user");
        });
    })
    //.AddUma(client =>
    //{
    //    client.ClientId = "client-role";
    //})
    .AddUma("Client")
    ;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseUma();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("api/authenticate", () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization();

app.MapGet("api/authorize", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("email_verified");
app.MapGet("api/attribute",[Authorize(Roles = "user")] () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"));

app.MapGet("api/role", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("role");

app.MapGet("api/uma", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireUmaAuthorization(resource: "user-resource", scope: "user-scope");

app.MapGet("api/umaa", [Permission(Resource = "user-resource", Scope = "user-scope", Roles = "user")] () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"));

app.MapGet("api/simple", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("Permission:user-resource,user-scope");


app.Run();
public partial class Program { }