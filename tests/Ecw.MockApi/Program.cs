var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/fhir/Bundle/{id}", (string id) =>
{
    var fileName = $"{id}.json";
    // Look for files in shared data directory, usually adjacent or copied
    // In a real scenario, we might configure this path via config
    // Here we assume the test execution context has access or we point to source
    
    // Attempt to locate the tests data folder relative to execution
    // This is brittle but sufficient for a POC in a known structure
    // C:\EpicalCDI\tests\Ecw.Tests\Data\FhirBundles
    var dataPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Ecw.Tests", "Data", "FhirBundles");
    
    // If running in container or published, this might change.
    // For now, looking at source structure relative to bin/Debug/net10.0
    
    var filePath = Path.Combine(dataPath, fileName);
    
    if (!File.Exists(filePath))
    {
        // Try looking in current directory (if content is copied)
         filePath = Path.Combine(AppContext.BaseDirectory, "Data", "FhirBundles", fileName);
    }

    if (!File.Exists(filePath))
    {
        return Results.NotFound($"Bundle {id} not found at {filePath}");
    }

    var json = File.ReadAllText(filePath);
    return Results.Text(json, "application/json");
});

app.Run();

// Make Program public for WebApplicationFactory
public partial class Program { }
