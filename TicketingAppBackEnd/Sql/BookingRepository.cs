using TicketingAppBackEnd.Models;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Sql.Interfaces;

namespace TicketingAppBackEnd.Sql
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DevContext _context; 

        public BookingRepository(DevContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int bookingId)
        {
            var booking =await _context.Bookings.FindAsync(bookingId);
            if (booking != null) _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByConcertId(int concertId)
        {
            var res = _context.Bookings.Where(c => c.ConcertId == concertId);
            foreach (var booking in res)
            {
                _context.Bookings.Remove(booking);
            }
            await _context.SaveChangesAsync();
        }

        public List<Booking> GetAll()
        {
            return _context.Bookings.ToList();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public int NbBookingsByConcertID(int concertId)
        {
            var nbBookingsByConcertId = _context.Bookings
                .Count(booking => booking.ConcertId == concertId);
            return nbBookingsByConcertId;
        }
    }
}
