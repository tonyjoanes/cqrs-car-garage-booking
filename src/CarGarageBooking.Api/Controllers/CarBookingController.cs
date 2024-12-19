// CarGarageBooking.Api/Controllers/CarBookingController.cs
using Microsoft.AspNetCore.Mvc;
using CarGarageBooking.Domain.Enums;
using CarGarageBooking.Domain.Interfaces;

namespace CarGarageBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarBookingController : ControllerBase
{
    private readonly IEventStore _eventStore;

    public CarBookingController(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var aggregate = CarBookingAggregate.Create(
            request.CustomerName,
            request.CarModel,
            request.Services
        );

        // Save the events
        await _eventStore.SaveEventsAsync(
            aggregate.Id,
            aggregate.GetUncommittedEvents()
        );

        // Clear uncommitted events after saving
        aggregate.ClearUncommittedEvents();

        return CreatedAtAction(
            nameof(GetBooking),
            new { id = aggregate.Id },
            new
            {
                aggregate.Id,
                aggregate.CustomerName,
                aggregate.CarModel,
                aggregate.Status,
                Services = aggregate.Services
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        var events = await _eventStore.GetEventsAsync(id);

        if (!events.Any())
            return NotFound();

        // Reconstruct aggregate using the new Reconstruct method
        var aggregate = CarBookingAggregate.Reconstruct(events);

        return Ok(new
        {
            aggregate.Id,
            aggregate.CustomerName,
            aggregate.CarModel,
            aggregate.Status,
            Services = aggregate.Services
        });
    }

    public class CreateBookingRequest
    {
        public required string CustomerName { get; set; }
        public required string CarModel { get; set; }
        public required List<ServiceType> Services { get; set; }
    }
}