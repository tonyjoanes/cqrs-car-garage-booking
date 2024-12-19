// CarGarageBooking.Domain/Interfaces/IEventStore.cs
using CarGarageBooking.Domain.Models;

namespace CarGarageBooking.Domain.Interfaces;

public interface ICarBookingRepository
{
    Task<CarBooking> GetByIdAsync(Guid id);
    Task CreateAsync(CarBooking booking);
    Task UpdateAsync(CarBooking booking);
}