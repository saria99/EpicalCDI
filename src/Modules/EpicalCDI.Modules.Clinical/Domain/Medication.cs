using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Clinical.Domain;

public class Medication : Entity<long>, ITenantEntity
{
    public HospitalId HospitalId { get; private set; }
    public long EncounterId { get; private set; }
    public string MedicationName { get; private set; } = default!;
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public string HashChecksum { get; private set; } = default!;
    public DateTime LastUpdatedUtc { get; private set; }

    private Medication() { } // EF Core

    public Medication(HospitalId hospitalId, long encounterId, string medicationName, DateTime startTime, DateTime? endTime, string hashChecksum)
    {
        HospitalId = hospitalId;
        EncounterId = encounterId;
        MedicationName = medicationName;
        StartTime = startTime;
        EndTime = endTime;
        HashChecksum = hashChecksum;
        LastUpdatedUtc = DateTime.UtcNow;
    }

    public void Update(DateTime? endTime, string hashChecksum)
    {
        EndTime = endTime;
        HashChecksum = hashChecksum;
        LastUpdatedUtc = DateTime.UtcNow;
    }
}
