using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace Ecw.Tests.Fixtures;

public class FhirFixture : IDisposable
{
    public Bundle Bundle01 { get; private set; }
    public Bundle Bundle02 { get; private set; }
    public Bundle Bundle03 { get; private set; }

    public FhirFixture()
    {
        var deserializer = new FhirJsonDeserializer();

        Bundle01 = LoadBundle(deserializer, "bundle-01-basic.json");
        Bundle02 = LoadBundle(deserializer, "bundle-02-inpatient.json");
        Bundle03 = LoadBundle(deserializer, "bundle-03-emergency.json");
    }

    private Bundle LoadBundle(FhirJsonDeserializer deserializer, string fileName)
    {
        // Adjust path to point to the Data/FhirBundles location. 
        // When running tests, the execution directory might be bin/Debug/net10.0
        // We need to resolve the path relative to the test project root or copy content to output.
        // For simplicity, let's assume the files are copied to output or we look up the tree.
        
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "FhirBundles", fileName);
        
        // Fallback for development time if CopyToOutput isn't set yet (though we should set it)
        if (!File.Exists(path))
        {
             path = Path.Combine("..", "..", "..", "Data", "FhirBundles", fileName);
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Constructed path: {path}", fileName);
        }

        string json = File.ReadAllText(path);
        return deserializer.Deserialize<Bundle>(json);
    }

    public void Dispose()
    {
        // Cleanup if necessary
    }
}
