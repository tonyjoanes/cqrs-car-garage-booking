// CarGarageBooking.Domain/Events/BookingEvents.cs
namespace CarGarageBooking.Domain.Events;

using CarGarageBooking.Domain.Enums;

public class ServiceAddedEvent : DomainEvent
{
    public ServiceType NewService { get; set; }
}