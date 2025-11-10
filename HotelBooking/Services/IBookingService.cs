using HotelBooking.Models;

namespace HotelBooking.Services;

public interface IBookingService
{
    Task<Booking> BookRoomAsync(string hotelName, string roomNumber, DateTime checkIn, DateTime checkOut, int numberOfGuests);
    string GenerateBookingReference();
}