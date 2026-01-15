using System;
using System.IO;
using System.Linq;
using Ecw.Tests.Fixtures;
using FluentAssertions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Xunit;

namespace Ecw.Tests;

public class NegativeTests
{
    private string GetFilePath(string fileName)
    {
         string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "FhirBundles", fileName);
        
        if (!File.Exists(path))
        {
             path = Path.Combine("..", "..", "..", "Data", "FhirBundles", fileName);
        }
        return path;
    }

    [Fact]
    public void Parse_EmptyBundle_ShouldReturnValidBundleWithNoEntries()
    {
        // Arrange
        var parser = new FhirJsonDeserializer();
        string json = File.ReadAllText(GetFilePath("bundle-empty.json"));

        // Act
        var bundle = parser.Deserialize<Bundle>(json);

        // Assert
        bundle.Should().NotBeNull();
        bundle.Entry.Should().BeEmpty();
    }

    [Fact]
    public void Parse_MissingPatient_ShouldReturnBundleWithoutPatient()
    {
        // Arrange
        var parser = new FhirJsonDeserializer();
        string json = File.ReadAllText(GetFilePath("bundle-missing-patient.json"));

        // Act
        var bundle = parser.Deserialize<Bundle>(json);

        // Assert
        bundle.Should().NotBeNull();
        bundle.Entry.Should().NotBeEmpty();
        
        var patient = bundle.Entry.Select(e => e.Resource).OfType<Patient>().FirstOrDefault();
        patient.Should().BeNull();
    }

    [Fact]
    public void Parse_MalformedJson_ShouldThrowException()
    {
        // Arrange
        var parser = new FhirJsonDeserializer();
        string json = File.ReadAllText(GetFilePath("malformed.json"));

        // Act
        Action act = () => parser.Deserialize<Bundle>(json);

        // Assert
        act.Should().Throw<Exception>(); // FhirJsonParser throws generic System.FormatException or similar, FormatException is an Exception
    }
}
