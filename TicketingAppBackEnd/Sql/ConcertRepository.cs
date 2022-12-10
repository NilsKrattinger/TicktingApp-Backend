using TicketingAppBackEnd.Models;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Sql.Interfaces;

namespace TicketingAppBackEnd.Sql
{
    public class ConcertRepository : IConcertRepository
    {

        private readonly DevContext _context;

        public ConcertRepository(DevContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Concert concert)
        {
            _context.Concerts.Add(concert);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int concertId)
        {
            var concert = await _context.Concerts.FindAsync(concertId);
            if (concert != null) _context.Concerts.Remove(concert);
            await _context.SaveChangesAsync();
        }

        public List<Concert> GetAll()
        {
            var res =  _context.Concerts.ToList();
            return res;
        }

        public Concert GetById(int concertId)
        {
            return _context.Concerts.Find(concertId);
        }

        public async Task UpdateAsync(Concert concert)
        {
            _context.Concerts.Update(concert);
            await _context.SaveChangesAsync();
        }
    }
}
