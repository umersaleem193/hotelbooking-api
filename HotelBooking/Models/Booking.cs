using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models;

public class Booking
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(20)]
    public string BookingReference { get; set; } = string.Empty;
    
    [Required]
    public DateTime CheckInDate { get; set; }
    
    [Required]
    public DateTime CheckOutDate { get; set; }
    
    [Range(1, 4)]
    public int NumberOfGuests { get; set; }
    
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}