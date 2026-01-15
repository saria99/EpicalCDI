using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Onboarding.Domain;

public class HospitalCredential : Entity<int>, ITenantEntity
{
    public HospitalId HospitalId { get; private set; }
    public CredentialType CredentialType { get; private set; }
    public string SecretReference { get; private set; } = default!;
    public DateTime RotationDateUtc { get; private set; }
    public bool IsActive { get; private set; }

    private HospitalCredential() { }

    public HospitalCredential(HospitalId hospitalId, CredentialType credentialType, string secretReference, DateTime rotationDateUtc)
    {
        HospitalId = hospitalId;
        CredentialType = credentialType;
        SecretReference = secretReference;
        RotationDateUtc = rotationDateUtc;
        IsActive = true;
    }
}
