using CarGarageBooking.Domain.Enums;
using CarGarageBooking.Domain.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class CarBookingAggregate
{
    // Private fields instead of properties
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    private Guid _id = Guid.NewGuid();
    private string _customerName;
    private string _carModel;
    private BookingStatus _status;
    private List<ServiceType> _services;

    // Readonly properties (no setters)
    public Guid Id => _id;
    public string CustomerName => _customerName;
    public string CarModel => _carModel;
    public BookingStatus Status => _status;
    public IReadOnlyList<ServiceType> Services => _services.AsReadOnly();
    private readonly List<DomainEvent> _uncommittedEvents = new List<DomainEvent>();

    // Private parameterless constructor for reconstruction
    private CarBookingAggregate() 
    {
        _services = new List<ServiceType>();
    }

    // Existing static factory method
    public static CarBookingAggregate Create(string customerName, string carModel, List<ServiceType> services)
    {
        var aggregate = new CarBookingAggregate();
        aggregate.RaiseEvent(new BookingCreatedEvent
        {
            AggregateId = Guid.NewGuid(),
            CustomerName = customerName,
            CarModel = carModel,
            Services = services
        });
        return aggregate;
    }

    // Method to reconstruct from events
    public static CarBookingAggregate Reconstruct(IEnumerable<DomainEvent> events)
    {
        var aggregate = new CarBookingAggregate();
        foreach (var domainEvent in events.OrderBy(e => e.Timestamp))
        {
            aggregate.Apply(domainEvent);
        }
        return aggregate;
    }

    // Private method to apply events
    private void Apply(DomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case BookingCreatedEvent e:
                _id = e.AggregateId;
                _customerName = e.CustomerName;
                _carModel = e.CarModel;
                _services = new List<ServiceType>(e.Services);
                _status = BookingStatus.Pending;
                break;
            case BookingStatusChangedEvent e:
                _status = e.NewStatus;
                break;
            case ServiceAddedEvent e:
                _services.Add(e.NewService);
                break;
        }
    }

    // Existing methods for changing state
    public void ChangeStatus(BookingStatus newStatus)
    {
        RaiseEvent(new BookingStatusChangedEvent
        {
            AggregateId = _id,
            NewStatus = newStatus
        });
    }

    public void AddService(ServiceType newService)
    {
        RaiseEvent(new ServiceAddedEvent
        {
            AggregateId = _id,
            NewService = newService
        });
    }

    // Existing event raising method
    private void RaiseEvent(DomainEvent domainEvent)
    {
        // Ensure the AggregateId is set
        domainEvent.AggregateId = _id;
        
        _uncommittedEvents.Add(domainEvent);
        Apply(domainEvent);
    }

    public IEnumerable<DomainEvent> GetUncommittedEvents()
    {
        return _uncommittedEvents.AsReadOnly();
    }

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }
}