using Microsoft.AspNetCore.Mvc;
using HotelBooking.Repositories;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly IHotelRepository _hotelRepository;

    public DataController(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    [HttpPost("seed")]
    public async Task<ActionResult> SeedData()
    {
        try
        {
            await _hotelRepository.SeedDataAsync();
            return Ok("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error seeding database: {ex.Message}");
        }
    }

    [HttpPost("reset")]
    public async Task<ActionResult> ResetData()
    {
        try
        {
            await _hotelRepository.ResetDataAsync();
            return Ok("Database reset successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error resetting database: {ex.Message}");
        }
    }
}