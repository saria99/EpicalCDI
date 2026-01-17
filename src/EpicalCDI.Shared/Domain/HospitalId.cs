
namespace EpicalCDI.Shared.Domain;

public record struct HospitalId(Guid Value)
{
    private Guid guid = Value;

}