using EpicalCDI.Shared.Domain;

namespace EpicalCDI.Shared.Abstractions;

public interface ITenantEntity
{
    HospitalId HospitalId { get; }
}
