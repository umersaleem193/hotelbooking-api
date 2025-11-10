using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models;

public class Hotel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}