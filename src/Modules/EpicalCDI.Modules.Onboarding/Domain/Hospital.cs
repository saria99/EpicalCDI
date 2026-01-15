using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Onboarding.Domain;

public class Hospital : Entity<HospitalId>, IAggregateRoot
{
    public string HospitalCode { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string TimeZone { get; private set; } = default!;
    public ExternalSystemType ExternalSystemType { get; private set; }
    public HospitalStatus Status { get; private set; }
    public DateTime CreatedUtc { get; private set; }
    public DateTime? UpdatedUtc { get; private set; }

    private Hospital() { } // EF Core

    public Hospital(HospitalId id, string hospitalCode, string name, string timeZone, ExternalSystemType systemType)
    {
        Id = id;
        HospitalCode = hospitalCode;
        Name = name;
        TimeZone = timeZone;
        ExternalSystemType = systemType;
        Status = HospitalStatus.Onboarding;
        CreatedUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = HospitalStatus.Active;
        UpdatedUtc = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = HospitalStatus.Suspended;
        UpdatedUtc = DateTime.UtcNow;
    }
}
