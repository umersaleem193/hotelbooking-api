using HotelBooking.Models;

namespace HotelBooking.Repositories;

public interface IHotelRepository
{
    Task<Hotel?> GetHotelByNameAsync(string name);
    Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int numberOfGuests);
    Task<Booking?> GetBookingByReferenceAsync(string bookingReference);
    Task<Booking> CreateBookingAsync(Booking booking);
    Task SeedDataAsync();
    Task ResetDataAsync();
}