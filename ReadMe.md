**Hotel Booking Web API**
This project is a RESTful **Hotel Room Booking API** built with **ASP.NET Core 8.0** and **Entity Framework Core**.  
It allows users to search hotels, find available rooms, and make bookings while enforcing key business rules.


**Implementation:**
- **Business Rules Implemented:**
  - Rooms cannot be double-booked for overlapping dates.
  - Guests do not change rooms mid-stay.
  - Room capacity is enforced.
  - Unique booking references are generated automatically.
- **Endpoints Provided:**
  - `GET /api/Hotels` – find hotels by name.
  - `GET /api/Hotels/available-rooms` – find all available rooms across hotels for a given date range.
  - `POST /api/Bookings` – create a new booking.
  - `GET /api/Bookings/{reference}` – retrieve booking details by booking reference.
  - `POST /api/Data/seed` – populate the database with sample hotels and rooms.
  - `POST /api/Data/reset` – clear all data for re-seeding.
- **Swagger (OpenAPI)** enabled for easy testing.
- **Database:** Azure SQL (configured via EF Core and connection string in `appsettings.json`).
