using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Onboarding.Domain;

public class HospitalDataScope : Entity<int>, ITenantEntity
{
    public HospitalId HospitalId { get; private set; }
    public DataDomain DataDomain { get; private set; }
    public bool Enabled { get; private set; }

    private HospitalDataScope() { }

    public HospitalDataScope(HospitalId hospitalId, DataDomain dataDomain, bool enabled = true)
    {
        HospitalId = hospitalId;
        DataDomain = dataDomain;
        Enabled = enabled;
    }
}
