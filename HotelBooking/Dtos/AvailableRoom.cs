using HotelBooking.Models;

namespace HotelBooking.Dtos;

public record AvailableRoomDto(
    string HotelName,
    string RoomNumber,
    RoomType RoomType,
    int Capacity
);
