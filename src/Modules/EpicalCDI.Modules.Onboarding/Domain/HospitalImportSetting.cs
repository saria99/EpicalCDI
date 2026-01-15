using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Onboarding.Domain;

public class HospitalImportSetting : Entity<int>, ITenantEntity
{
    public HospitalId HospitalId { get; private set; }
    public UpdateStrategy UpdateStrategy { get; private set; }
    public bool AllowDeletes { get; private set; }
    public int DefaultLookbackDays { get; private set; }
    public int MaxBatchSize { get; private set; }

    private HospitalImportSetting() { }

    public HospitalImportSetting(HospitalId hospitalId, UpdateStrategy updateStrategy, bool allowDeletes, int defaultLookbackDays, int maxBatchSize)
    {
        HospitalId = hospitalId;
        UpdateStrategy = updateStrategy;
        AllowDeletes = allowDeletes;
        DefaultLookbackDays = defaultLookbackDays;
        MaxBatchSize = maxBatchSize;
    }
}
