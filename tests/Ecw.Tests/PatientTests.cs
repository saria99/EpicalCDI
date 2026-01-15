using Ecw.Tests.Fixtures;
using FluentAssertions;
using Hl7.Fhir.Model;
using Xunit;

namespace Ecw.Tests;

public class PatientTests : IClassFixture<FhirFixture>
{
    private readonly FhirFixture _fixture;

    public PatientTests(FhirFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Patient_ShouldHaveName()
    {
        // Arrange
        var bundle = _fixture.Bundle01;
        var patient = bundle.Entry.Select(e => e.Resource).OfType<Patient>().FirstOrDefault();

        // Assert
        patient.Should().NotBeNull();
        patient!.Name.Should().NotBeEmpty();
        patient.Name.First().Family.Should().NotBeNullOrEmpty();
        patient.Name.First().Given.Should().NotBeEmpty();
    }

    [Fact]
    public void Patient_ShouldHaveDateOfBirth()
    {
        // Arrange
        var bundle = _fixture.Bundle01;
        var patient = bundle.Entry.Select(e => e.Resource).OfType<Patient>().FirstOrDefault();

        // Assert
        patient.Should().NotBeNull();
        patient!.BirthDate.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Patient_ShouldHaveGender()
    {
        // Arrange
        var bundle = _fixture.Bundle02;
        var patient = bundle.Entry.Select(e => e.Resource).OfType<Patient>().FirstOrDefault();

        // Assert
        patient.Should().NotBeNull();
        patient!.Gender.Should().NotBeNull();
        patient.Gender.Should().BeOneOf(AdministrativeGender.Male, AdministrativeGender.Female, AdministrativeGender.Other, AdministrativeGender.Unknown);
    }
}
