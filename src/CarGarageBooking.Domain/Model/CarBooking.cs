// CarGarageBooking.Domain/Models/CarBooking.cs
using CarGarageBooking.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarGarageBooking.Domain.Models;

public class CarBooking
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string CarModel { get; set; }
    public BookingStatus Status { get; set; }
    public List<ServiceType> Services { get; set; }
}