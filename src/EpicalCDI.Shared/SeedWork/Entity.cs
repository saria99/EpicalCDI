namespace EpicalCDI.Shared.SeedWork;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;

    private List<INotification>? _domainEvents;
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly() ?? (IReadOnlyCollection<INotification>)new List<INotification>();

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents = _domainEvents ?? new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}

// Simple marker for MediatR notifications if you use them, or just a placeholder for now
public interface INotification { }
