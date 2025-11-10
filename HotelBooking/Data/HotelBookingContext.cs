using Microsoft.EntityFrameworkCore;
using HotelBooking.Models;

namespace HotelBooking.Data;

public class HotelBookingContext : DbContext
{
    public HotelBookingContext(DbContextOptions<HotelBookingContext> options) : base(options)
    {
    }
    
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(h => h.Name).IsUnique();
        });
        
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.RoomNumber).IsRequired().HasMaxLength(10);
            entity.HasOne(r => r.Hotel)
                  .WithMany(h => h.Rooms)
                  .HasForeignKey(r => r.HotelId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(r => new { r.HotelId, r.RoomNumber }).IsUnique();
        });
        
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.BookingReference).IsRequired().HasMaxLength(20);
            entity.Property(b => b.CheckInDate).IsRequired();
            entity.Property(b => b.CheckOutDate).IsRequired();
            entity.Property(b => b.NumberOfGuests).IsRequired();
            entity.Property(b => b.CreatedAt).IsRequired();
            entity.HasOne(b => b.Room)
                  .WithMany(r => r.Bookings)
                  .HasForeignKey(b => b.RoomId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(b => b.BookingReference).IsUnique();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}