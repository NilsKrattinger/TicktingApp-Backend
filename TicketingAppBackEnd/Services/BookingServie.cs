using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Sql.Interfaces;

namespace TicketingAppBackEnd.Services;

public class BookingService : TicketingAppBackEnd.Protos.BookingService.BookingServiceBase
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public override async Task<CustomOperationReply> AddBooking(Booking request, ServerCallContext context)
    {
        request.DateUTC = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime());
        await _bookingRepository.AddAsync(request);

        return new CustomOperationReply
        {
            Code = 0
        };
    }

    public override Task<GetAllBookingReply> GetAllBookings(Empty request, ServerCallContext context)
    {
        var bookings = _bookingRepository.GetAll();
        var reply = new GetAllBookingReply();
        reply.Booking.AddRange(bookings);
        return Task.FromResult(reply);
    }
}