using Microsoft.AspNetCore.Mvc;
using HotelBooking.Models;
using HotelBooking.Dtos;
using HotelBooking.Repositories;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelRepository _hotelRepository;

    public HotelsController(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Hotel>> GetHotel(string name)
    {
        var hotel = await _hotelRepository.GetHotelByNameAsync(name);
        if (hotel == null)
            return NotFound($"Hotel '{name}' not found.");

        return Ok(new HotelDto(hotel.Id, hotel.Name));
    }

    [HttpGet("available-rooms")]
    public async Task<ActionResult<List<AvailableRoomDto>>> GetAvailableRooms(
        [FromQuery] DateTime checkIn,
        [FromQuery] DateTime checkOut,
        [FromQuery] int numberOfGuests)
    {
        if (checkIn >= checkOut)
            return BadRequest("Check-in date must be before check-out date.");

        if (numberOfGuests <= 0 || numberOfGuests > 4)
            return BadRequest("Number of guests must be between 1 and 4.");

        var availableRooms = await _hotelRepository.GetAvailableRoomsAsync(checkIn, checkOut, numberOfGuests);

        var result = availableRooms.Select(r => new AvailableRoomDto(
            r.Hotel.Name,
            r.RoomNumber,
            r.RoomType,
            r.Capacity
        ));

        return Ok(result);
    }

}