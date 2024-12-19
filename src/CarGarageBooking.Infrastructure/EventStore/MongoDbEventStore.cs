// CarGarageBooking.Infrastructure/EventStore/MongoDbEventStore.cs
using MongoDB.Driver;
using CarGarageBooking.Domain.Interfaces;
using CarGarageBooking.Domain.Events;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

namespace CarGarageBooking.Infrastructure.EventStore;

public class MongoDbEventStore : IEventStore
{
    private readonly IMongoCollection<DomainEvent> _events;

    public MongoDbEventStore(IMongoDatabase database)
    {
        _events = database.GetCollection<DomainEvent>("events");
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<DomainEvent> events)
    {
        await _events.InsertManyAsync(events);
    }

    public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
    {
        return await _events
            .Find(e => e.AggregateId == aggregateId)
            .ToListAsync();
    }
}