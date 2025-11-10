using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Models;

public class Room
{
    public int Id { get; set; }
    
    [Required]
    public string RoomNumber { get; set; } = string.Empty;
    
    public RoomType RoomType { get; set; }
    
    public int Capacity => (int)RoomType;
    
    public int HotelId { get; set; }

    public Hotel Hotel { get; set; } = null!;
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}