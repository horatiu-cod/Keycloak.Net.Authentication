var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.EnableSubstitutions();

builder.Services
    .AddKeyCloakAuthentication()
    //.AddKeyCloakJwtBearerOptions("appsettings_section_name")

    
    .AddKeyCloakJwtBearerOptions("appsettings_section_name", o =>
    {
        o.Audience = "account";
        o.SaveToken = true;

        o.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(30);
    })
    

    /*
    .AddKeyCloakJwtBearerOptions(c =>
    {
        c.Authority = "https://localhost:8843/realms/Test";
        c.ValidAudience = "account";
    })
    */
    ;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
