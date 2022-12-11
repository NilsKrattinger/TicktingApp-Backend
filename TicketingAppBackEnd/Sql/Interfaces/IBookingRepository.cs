
using TicketingAppBackEnd.Protos;

namespace TicketingAppBackEnd.Sql.Interfaces;

public interface IBookingRepository
{
    public Task AddAsync(Booking booking);
    public Task UpdateAsync(Booking booking);
    public Task DeleteAsync(int bookingId);
    public List<Booking> GetAll();
    public Task DeleteByConcertId(int concertId);
    public int NbBookingsByConcertID(int concertId);
}