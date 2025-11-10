namespace HotelBooking.Dtos;

public record BookingDto(
    string BookingReference,
    string HotelName,
    string RoomNumber,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    int NumberOfGuests
);
