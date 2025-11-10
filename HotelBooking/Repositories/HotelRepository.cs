using HotelBooking.Data;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly HotelBookingContext _context;

    public HotelRepository(HotelBookingContext context)
    {
        _context = context;
    }

    public async Task<Hotel?> GetHotelByNameAsync(string name)
    {
        return await _context.Hotels
            .Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Name.ToLower() == name.ToLower());
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int numberOfGuests)
    {
        var rooms = await _context.Rooms
            .Include(r => r.Hotel)
            .Include(r => r.Bookings)
            .ToListAsync();

        return rooms
            .Where(r => r.Capacity >= numberOfGuests &&
                        !r.Bookings.Any(b => checkIn < b.CheckOutDate && checkOut > b.CheckInDate)).ToList();
    }

    public async Task<Booking?> GetBookingByReferenceAsync(string bookingReference)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b => b.BookingReference == bookingReference);
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return await GetBookingByReferenceAsync(booking.BookingReference) ?? booking;
    }

    public async Task SeedDataAsync()
    {
        var hotelsToSeed = new List<string>
        {
        "HolidayInn",
        "Marriot",
        "Sheraton"
        };

        foreach (var hotelName in hotelsToSeed)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Name == hotelName);

            if (hotel == null)
            {
                hotel = new Hotel { Name = hotelName };
                _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();
            }

            var roomsToAdd = new List<Room>
            {
            new() { HotelId = hotel.Id, RoomNumber = "101", RoomType = RoomType.Single },
            new() { HotelId = hotel.Id, RoomNumber = "102", RoomType = RoomType.Single },
            new() { HotelId = hotel.Id, RoomNumber = "201", RoomType = RoomType.Double },
            new() { HotelId = hotel.Id, RoomNumber = "202", RoomType = RoomType.Double },
            new() { HotelId = hotel.Id, RoomNumber = "301", RoomType = RoomType.Deluxe },
            new() { HotelId = hotel.Id, RoomNumber = "302", RoomType = RoomType.Deluxe }
            };

            foreach (var room in roomsToAdd)
            {
                bool exists = await _context.Rooms
                    .AnyAsync(r => r.HotelId == hotel.Id && r.RoomNumber == room.RoomNumber);
                if (!exists)
                    _context.Rooms.Add(room);
            }
        }

        await _context.SaveChangesAsync();
    }


    public async Task ResetDataAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }
}