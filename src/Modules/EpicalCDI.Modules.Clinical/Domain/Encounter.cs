using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Clinical.Domain;

public class Encounter : Entity<long>, ITenantEntity
{
    public HospitalId HospitalId { get; private set; }
    public string ExternalEncounterId { get; private set; } = default!;
    public string PatientExternalId { get; private set; } = default!;
    public DateTime AdmitDateTime { get; private set; }
    public DateTime? DischargeDateTime { get; private set; }
    public DateTime LastUpdatedUtc { get; private set; }

    // Navigation property for related data
    private readonly List<Observation> _observations = new();
    public IReadOnlyCollection<Observation> Observations => _observations.AsReadOnly();

    private readonly List<Medication> _medications = new();
    public IReadOnlyCollection<Medication> Medications => _medications.AsReadOnly();

    private Encounter() { } // EF Core

    public Encounter(HospitalId hospitalId, string externalEncounterId, string patientExternalId, DateTime admitDateTime)
    {
        HospitalId = hospitalId;
        ExternalEncounterId = externalEncounterId;
        PatientExternalId = patientExternalId;
        AdmitDateTime = admitDateTime;
        LastUpdatedUtc = DateTime.UtcNow;
    }

    public void UpdateDischargeDate(DateTime? dischargeDate)
    {
        DischargeDateTime = dischargeDate;
        LastUpdatedUtc = DateTime.UtcNow;
    }
}
