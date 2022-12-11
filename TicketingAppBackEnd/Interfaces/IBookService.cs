using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TicketingAppBackEnd.Protos;

namespace TicketingAppBackEnd.Interfaces;

public interface IBookService
{
    public Task<CustomOperationReply> AddBooking(Booking request, ServerCallContext? context = null);

    public Task<GetAllBookingReply> GetAllBookings(Empty request, ServerCallContext? context = null);
}