using Microsoft.EntityFrameworkCore;
using TicketingLib.Model;

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
