using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TicketingAppBackEnd.Interfaces;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Sql.Interfaces;

namespace TicketingAppBackEnd.Services;

public class BookingService : TicketingAppBackEnd.Protos.BookingService.BookingServiceBase, IBookService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IConcertRepository _concertRepository;
    private MailService.MailServiceClient _mailServiceClient;


    public BookingService(IBookingRepository bookingRepository, IConcertRepository concertRepository, MailService.MailServiceClient mailService)
    {
        _bookingRepository = bookingRepository;
        _concertRepository = concertRepository;
        _mailServiceClient = mailService;
    }

    public override Task<GetAllBookingReply> GetAllBookings(Empty request, ServerCallContext? context)
    {
        return GetAllBookings(request);
    }


    public override async Task<CustomOperationReply> AddBooking(Booking request, ServerCallContext? context)
    {
        
        var concert = _concertRepository.GetById(request.ConcertId).Concert;
        if (concert == null)
            return new CustomOperationReply
            {
                Code = 2,
                Message = "Concert do not seem to exist"
            };

        
        var nbBookings = _bookingRepository.NbBookingsByConcertID(request.ConcertId);
        if (concert.Place - nbBookings <= 0)
            return new CustomOperationReply
            {
                Code = 1,
                Message = "Missing places"
            };

        request.DateUTC = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime());
        await _bookingRepository.AddAsync(request);
        _mailServiceClient.SendMail(new mail
        {
            Target = request.Email,
            Sender = "mail@Ticekting.com",
            Subject = "Booking for " + concert.Artist,
            Body = "Place successfully Booked",
        });
        return new CustomOperationReply
        {
            Code = 0
        };
    }

    public Task<GetAllBookingReply> GetAllBookings(Empty request)
    {
        var bookings = _bookingRepository.GetAll();
        var reply = new GetAllBookingReply();
        reply.Booking.AddRange(bookings);
        return Task.FromResult(reply);
    }
}