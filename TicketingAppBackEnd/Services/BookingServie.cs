using Grpc.Core;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Sql.Interfaces;

namespace TicketingAppBackEnd.Services
{
    public class BookingService : TicketingAppBackEnd.Protos.BookingService.BookingServiceBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<CustomOperationReply> AddReservation(AddBookingRequest request, ServerCallContext context)
        {
            var reservation = new Booking()
            {
                Id = request.Id, 
                Price = request.Price, 
                Date = request.Date,
                Email = request.Email, 
                Payment = request.Payment,
                ConcertId = request.ConcertId
            };
            await _bookingRepository.AddAsync(reservation);

            return new CustomOperationReply
            {
                Code = "OK + reservation.Id"
            };
        }
    }
}
