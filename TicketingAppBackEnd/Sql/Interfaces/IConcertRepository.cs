using TicketingAppBackEnd.Protos;

namespace TicketingAppBackEnd.Sql.Interfaces
{
    public interface IConcertRepository
    {
        public Task AddAsync(Concert concert);
        public Task UpdateAsync(Concert concert);
        public Task DeleteAsync(int concertId);
        public List<Concert> GetAll();
        public Concert GetById(int concertId);
    }
}
