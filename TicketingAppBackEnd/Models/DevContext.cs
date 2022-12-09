using Microsoft.EntityFrameworkCore;
using TicketingAppBackEnd.Protos;

namespace TicketingAppBackEnd.Models
{
    public class DevContext : DbContext
    {
        public DevContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Concert> Concerts { get; set; }

    }
}
