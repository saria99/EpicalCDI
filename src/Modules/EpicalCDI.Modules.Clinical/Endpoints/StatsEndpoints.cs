using EpicalCDI.Modules.Clinical.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EpicalCDI.Modules.Clinical.Endpoints;

public static class StatsEndpoints
{
    public static void MapClinicalStatsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stats")
            .WithTags("Clinical Stats")
            .RequireAuthorization();

        group.MapGet("/summary", async (ClinicalDbContext db) =>
        {
            // Simple aggregation for now
            // var totalPatients = await db.Patients.CountAsync(); // Patients table not yet defined in Clinical module
            var totalEncounters = await db.Encounters.CountAsync();
            var totalObservations = await db.Observations.CountAsync();

            return new ClinicalStatsValidation(0, totalEncounters, totalObservations);
        })
        .WithName("GetClinicalStats");
    }
}

public record ClinicalStatsValidation(int TotalPatients, int TotalEncounters, int TotalObservations);
