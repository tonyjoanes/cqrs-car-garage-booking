// CarGarageBooking.Domain/Events/DomainEvent.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarGarageBooking.Domain.Events;
public abstract class DomainEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Guid AggregateId { get; set; }
}
