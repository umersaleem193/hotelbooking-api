using HotelBooking.Models;

namespace HotelBooking.Dtos;

public record RoomDto(string RoomNumber, RoomType RoomType, int Capacity);
