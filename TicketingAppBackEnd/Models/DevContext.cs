using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using TicketingAppBackEnd.Protos;

namespace TicketingAppBackEnd.Models;

public class DevContext : DbContext
{
    public DevContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .HasKey(b => b.Id)
            .HasName("PrimaryKey_Booking");
        modelBuilder
            .Entity<Booking>()
            .Property(e => e.DateUTC)
            .HasConversion(
                v => v.ToDateTime(),
                v => v.ToUniversalTime().ToTimestamp());
    }

    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Concert> Concerts { get; set; }

}