// CarGarageBooking.Domain/Events/BookingEvents.cs
namespace CarGarageBooking.Domain.Events;

using CarGarageBooking.Domain.Enums;

public class BookingCreatedEvent : DomainEvent
{
    public string CustomerName { get; set; }
    public string CarModel { get; set; }
    public List<ServiceType> Services { get; set; }
}
