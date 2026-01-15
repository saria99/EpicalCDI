using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Onboarding.Domain;

public class HospitalCodeMapping : Entity<int>, ITenantEntity
{
    public HospitalId HospitalId { get; private set; }
    public string SourceSystem { get; private set; } = default!;
    public string SourceCode { get; private set; } = default!;
    public string NormalizedCode { get; private set; } = default!;
    public CodeSystemType CodeSystem { get; private set; }
    public bool IsActive { get; private set; }

    private HospitalCodeMapping() { }

    public HospitalCodeMapping(HospitalId hospitalId, string sourceSystem, string sourceCode, string normalizedCode, CodeSystemType codeSystem)
    {
        HospitalId = hospitalId;
        SourceSystem = sourceSystem;
        SourceCode = sourceCode;
        NormalizedCode = normalizedCode;
        CodeSystem = codeSystem;
        IsActive = true;
    }
}
