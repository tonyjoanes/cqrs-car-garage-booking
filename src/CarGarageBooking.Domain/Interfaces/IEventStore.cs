// CarGarageBooking.Domain/Interfaces/IEventStore.cs
using CarGarageBooking.Domain.Events;

namespace CarGarageBooking.Domain.Interfaces;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<DomainEvent> events);
    Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId);
}
