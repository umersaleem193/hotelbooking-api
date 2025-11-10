using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Repositories;

namespace HotelBooking.Services;

public class BookingService : IBookingService
{
    private readonly HotelBookingContext _context;
    private readonly IHotelRepository _hotelRepository;

    public BookingService(HotelBookingContext context, IHotelRepository hotelRepository)
    {
        _context = context;
        _hotelRepository = hotelRepository;
    }

    public async Task<Booking> BookRoomAsync(string hotelName, string roomNumber, DateTime checkIn, DateTime checkOut, int numberOfGuests)
    {
        if (checkIn >= checkOut)
            throw new ArgumentException("Check-in date must be before check-out date.");

        if (numberOfGuests <= 0 || numberOfGuests > 4)
            throw new ArgumentException("Number of guests must be between 1 and 4.");

        var hotel = await _hotelRepository.GetHotelByNameAsync(hotelName);
        if (hotel == null)
            throw new ArgumentException($"Hotel '{hotelName}' not found.");

        var room = await _context.Rooms
            .Include(r => r.Bookings)
            .FirstOrDefaultAsync(r => r.HotelId == hotel.Id && r.RoomNumber == roomNumber);

        if (room == null)
            throw new ArgumentException($"Room '{roomNumber}' not found in hotel '{hotelName}'.");

        if (room.Capacity < numberOfGuests)
            throw new ArgumentException($"Room capacity ({room.Capacity}) is less than number of guests ({numberOfGuests}).");

        var hasConflictingBooking = room.Bookings.Any(b => 
            checkIn < b.CheckOutDate && checkOut > b.CheckInDate);

        if (hasConflictingBooking)
            throw new ArgumentException($"Room '{roomNumber}' is not available for the selected dates.");

        var booking = new Booking
        {
            BookingReference = GenerateBookingReference(),
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            NumberOfGuests = numberOfGuests,
            RoomId = room.Id
        };

        return await _hotelRepository.CreateBookingAsync(booking);
    }

    public string GenerateBookingReference()
    {
        return $"BK{DateTime.UtcNow:yyyyMMddHHmm}{Random.Shared.Next(1000, 9999)}";
    }
}