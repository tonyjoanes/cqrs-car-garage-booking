// CarGarageBooking.Domain/Events/BookingEvents.cs
namespace CarGarageBooking.Domain.Events;

using CarGarageBooking.Domain.Enums;

public class BookingStatusChangedEvent : DomainEvent
{
    public BookingStatus NewStatus { get; set; }
}
