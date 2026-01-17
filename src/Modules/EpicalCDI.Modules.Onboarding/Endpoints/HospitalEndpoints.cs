using EpicalCDI.Modules.Onboarding.Domain;
using EpicalCDI.Modules.Onboarding.Infrastructure;
using EpicalCDI.Shared.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EpicalCDI.Modules.Onboarding.Endpoints;

public static class HospitalEndpoints
{
    public static void MapHospitalEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/hospitals")
            .WithTags("Hospitals")
            .RequireAuthorization();

        group.MapGet("/", async (OnboardingDbContext db) =>
        {
            return await db.Hospitals.AsNoTracking().ToListAsync();
        })
        .WithName("GetHospitals");

        group.MapGet("/{id}", async (Guid id, OnboardingDbContext db) =>
        {
            var hospital = await db.Hospitals.FindAsync(id);
            return hospital is not null ? Results.Ok(hospital) : Results.NotFound();
        })
        .WithName("GetHospitalById");

        group.MapPost("/", async (CreateHospitalRequest request, OnboardingDbContext db) =>
        {
            var hospitalId = new HospitalId(Guid.NewGuid());
            var hospital = new Hospital(
                hospitalId,
                request.HospitalCode,
                request.Name,
                request.TimeZone,
                request.ExternalSystemType
            );

            db.Hospitals.Add(hospital);
            await db.SaveChangesAsync();

            return Results.Created($"/api/hospitals/{hospitalId.Value}", hospital);
        })
        .WithName("CreateHospital");
    }
}

public record CreateHospitalRequest(string HospitalCode, string Name, string TimeZone, ExternalSystemType ExternalSystemType);
