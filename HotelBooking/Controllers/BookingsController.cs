using Microsoft.AspNetCore.Mvc;
using HotelBooking.Models;
using HotelBooking.Repositories;
using HotelBooking.Services;
using HotelBooking.Dtos;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IBookingService _bookingService;

    public BookingsController(IHotelRepository hotelRepository, IBookingService bookingService)
    {
        _hotelRepository = hotelRepository;
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> BookRoom([FromBody] BookRoomRequest request)
    {
        try
        {
            var booking = await _bookingService.BookRoomAsync(
                request.HotelName,
                request.RoomNumber,
                request.CheckInDate,
                request.CheckOutDate,
                request.NumberOfGuests);

            var bookingDto = new BookingDto(
                booking.BookingReference,
                booking.Room.Hotel.Name,
                booking.Room.RoomNumber,
                booking.CheckInDate,
                booking.CheckOutDate,
                booking.NumberOfGuests
            );

            return CreatedAtAction(
                nameof(GetBooking),
                new { bookingReference = booking.BookingReference },
                bookingDto
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("{bookingReference}")]
    public async Task<ActionResult<Booking>> GetBooking(string bookingReference)
    {
        var booking = await _hotelRepository.GetBookingByReferenceAsync(bookingReference);
        if (booking == null)
            return NotFound($"Booking '{bookingReference}' not found.");

        var bookingDto = new BookingDto(
        booking.BookingReference,
        booking.Room.Hotel.Name,
        booking.Room.RoomNumber,
        booking.CheckInDate,
        booking.CheckOutDate,
        booking.NumberOfGuests
        );

        return Ok(bookingDto);
    }
}

public class BookRoomRequest
{
    public string HotelName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
}