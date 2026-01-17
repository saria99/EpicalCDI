using EpicalCDI.Modules.Onboarding.Infrastructure;
using EpicalCDI.Modules.Clinical.Infrastructure;
using EpicalCDI.Modules.Onboarding.Endpoints;
using EpicalCDI.Modules.Clinical.Endpoints;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authSettings = builder.Configuration.GetSection("Authentication");
        options.Authority = authSettings["Authority"];
        options.Audience = authSettings["Audience"];
        options.RequireHttpsMetadata = bool.Parse(authSettings["RequireHttpsMetadata"] ?? "true");
    });

builder.Services.AddAuthorization();

builder.AddNpgsqlDbContext<OnboardingDbContext>("epicalcdi-db");
builder.AddNpgsqlDbContext<ClinicalDbContext>("epicalcdi-db");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHeaderPropagation();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapHospitalEndpoints();
app.MapClinicalStatsEndpoints();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
