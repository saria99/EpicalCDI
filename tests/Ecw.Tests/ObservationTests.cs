using Ecw.Tests.Fixtures;
using FluentAssertions;
using Hl7.Fhir.Model;
using Xunit;

namespace Ecw.Tests;

public class ObservationTests : IClassFixture<FhirFixture>
{
    private readonly FhirFixture _fixture;

    public ObservationTests(FhirFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Observation_ShouldHaveCode()
    {
        // Arrange
        var bundle = _fixture.Bundle01;
        var observation = bundle.Entry.Select(e => e.Resource).OfType<Observation>().FirstOrDefault();

        // Assert
        observation.Should().NotBeNull();
        observation!.Code.Should().NotBeNull();
        observation.Code.Coding.Should().NotBeEmpty();
        observation.Code.Coding.First().Code.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Observation_ShouldHaveValue()
    {
        // Arrange
        var bundle = _fixture.Bundle01; // Heart rate: 72
        var observation = bundle.Entry.Select(e => e.Resource).OfType<Observation>().FirstOrDefault();

        // Assert
        observation.Should().NotBeNull();
        observation!.Value.Should().NotBeNull();
        
        var quantity = observation.Value as Quantity;
        quantity.Should().NotBeNull();
        quantity!.Value.Should().Be(72);
    }

    [Fact]
    public void Observation_ShouldHaveEffectiveDate()
    {
        // Arrange
        var bundle = _fixture.Bundle03; // Emergency visit
        var observation = bundle.Entry.Select(e => e.Resource).OfType<Observation>().FirstOrDefault();

        // Assert
        observation.Should().NotBeNull();
        observation!.Effective.Should().NotBeNull();
        
        // In bundle-03, checking for effectiveDateTime
        var effectiveDateTime = observation.Effective as FhirDateTime;
        effectiveDateTime.Should().NotBeNull();
        effectiveDateTime!.Value.Should().NotBeNullOrEmpty();
    }
}
