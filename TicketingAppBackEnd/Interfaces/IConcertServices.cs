using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TicketingAppBackEnd.Protos;

namespace TicketingAppBackEnd.Interfaces;

public interface IConcertServices
{
    public Task<CustomOperationReply> AddConcert(Concert request, ServerCallContext? context = null);


    public Task<CustomOperationReply> DeleteConcert(Concert request, ServerCallContext? context = null);

    public Task<CustomOperationReply> UpdateConcert(Concert request, ServerCallContext? context = null);


    public Task<GetAllConcertsReply> GetAllConcerts(Empty request, ServerCallContext? context = null);


    public Task<ConcertReply> GetById(ConcertId request, ServerCallContext? context = null);


    public Task<CustomOperationReply> DeleteConcert(ConcertId request, ServerCallContext? context = null);
}