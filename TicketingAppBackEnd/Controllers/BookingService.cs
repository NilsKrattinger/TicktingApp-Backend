using Microsoft.AspNetCore.Mvc;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Sql.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketingAppBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        public List<Booking> Get()
        {
            return _bookingRepository.GetAll();
        }

        [HttpGet("{id}")]
        public Booking Get(int id)
        {
            return _bookingRepository.GetById(id);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] Booking reservation)
        {
            await _bookingRepository.AddAsync(reservation);
        }

        [HttpPut("{id}")]
        public async Task PutAsync(Booking reservation)
        {
            await _bookingRepository.UpdateAsync(reservation);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }
    }
}