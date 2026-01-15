using EpicalCDI.Shared.Abstractions;
using EpicalCDI.Shared.Domain;
using EpicalCDI.Shared.SeedWork;

namespace EpicalCDI.Modules.Onboarding.Domain;

public class HospitalIntegrationEndpoint : Entity<int>, ITenantEntity
{
    public HospitalId HospitalId { get; private set; }
    public IntegrationType IntegrationType { get; private set; }
    public string EndpointUrl { get; private set; } = default!;
    public ProtocolType Protocol { get; private set; }
    public bool IsActive { get; private set; }

    private HospitalIntegrationEndpoint() { }

    public HospitalIntegrationEndpoint(HospitalId hospitalId, IntegrationType integrationType, string endpointUrl, ProtocolType protocol)
    {
        HospitalId = hospitalId;
        IntegrationType = integrationType;
        EndpointUrl = endpointUrl;
        Protocol = protocol;
        IsActive = true;
    }
}
