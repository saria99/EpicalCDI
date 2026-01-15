using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Clinical.Domain;

public class Observation : Entity<long>, ITenantEntity
{
    public HospitalId HospitalId { get; private set; }
    public long EncounterId { get; private set; }
    public string Code { get; private set; } = default!;
    public string Value { get; private set; } = default!;
    public string? Unit { get; private set; }
    public DateTime ObservationTime { get; private set; }
    public string HashChecksum { get; private set; } = default!; // Storing as hex string
    public DateTime LastUpdatedUtc { get; private set; }

    private Observation() { } // EF Core

    public Observation(HospitalId hospitalId, long encounterId, string code, string value, string? unit, DateTime observationTime, string hashChecksum)
    {
        HospitalId = hospitalId;
        EncounterId = encounterId;
        Code = code;
        Value = value;
        Unit = unit;
        ObservationTime = observationTime;
        HashChecksum = hashChecksum;
        LastUpdatedUtc = DateTime.UtcNow;
    }

    public void Update(string value, string? unit, string hashChecksum)
    {
        Value = value;
        Unit = unit;
        HashChecksum = hashChecksum;
        LastUpdatedUtc = DateTime.UtcNow;
    }
}
