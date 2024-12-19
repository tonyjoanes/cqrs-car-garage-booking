// CarGarageBooking.Infrastructure/Configuration/MongoDbConfiguration.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using CarGarageBooking.Domain.Events;

namespace CarGarageBooking.Infrastructure.Configuration;

public static class MongoDbConfiguration
{
    public static void ConfigureMongoDbSerializers()
    {
        // Configure Guid serialization
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        // Optional: Register any custom serializers for your domain events
        BsonClassMap.RegisterClassMap<DomainEvent>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.Id);
        });
    }
}