using Ecw.Tests.Fixtures;
using FluentAssertions;
using Hl7.Fhir.Model;
using Xunit;

namespace Ecw.Tests;

public class EncounterTests : IClassFixture<FhirFixture>
{
    private readonly FhirFixture _fixture;

    public EncounterTests(FhirFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Encounter_ShouldHaveValidStatus()
    {
        // Arrange
        var bundle = _fixture.Bundle01;
        var encounter = bundle.Entry.Select(e => e.Resource).OfType<Encounter>().FirstOrDefault();

        // Assert
        encounter.Should().NotBeNull();
        encounter!.Status.Should().Be(Encounter.EncounterStatus.Finished); // Based on our sample data
    }

    [Fact]
    public void Encounter_ShouldReferencePatient()
    {
        // Arrange
        var bundle = _fixture.Bundle01;
        var encounter = bundle.Entry.Select(e => e.Resource).OfType<Encounter>().FirstOrDefault();
        var patient = bundle.Entry.Select(e => e.Resource).OfType<Patient>().FirstOrDefault();

        // Assert
        encounter.Should().NotBeNull();
        patient.Should().NotBeNull();

        encounter!.Subject.Should().NotBeNull();
        encounter.Subject.Reference.Should().Be("Patient/" + patient!.Id);
    }
}
