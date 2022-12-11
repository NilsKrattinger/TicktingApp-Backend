using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TicketingAppBackEnd.Interfaces;
using TicketingAppBackEnd.Sql.Interfaces;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Validator;

namespace TicketingAppBackEnd.Services;

public class ConcertService : TicketingAppBackEnd.Protos.ConcertService.ConcertServiceBase, IConcertServices
{
    private readonly IConcertRepository _concertRepository;
    private readonly IBookingRepository _bookingRepository;

    public ConcertService(IConcertRepository concertRepository,IBookingRepository bookingRepository)
    {
        _concertRepository = concertRepository;
        _bookingRepository = bookingRepository;
    }

    public override async Task<CustomOperationReply> AddConcert(Concert request, ServerCallContext? context)
    {
        var validator = new ConcertValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return new CustomOperationReply
            {
                Code = 3,
                Message = validation.Errors.ToString()
            };
        }

        await _concertRepository.AddAsync(request);

        return new CustomOperationReply
        {
            Code = 0,
            Message = request.Artist
        };
    }

    public async Task<CustomOperationReply> DeleteConcert(Concert request, ServerCallContext? context)
    {

        var validator = new ConcertValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return new CustomOperationReply
            {
                Code = 3,
                Message = validation.Errors.ToString()
            };
        }

        await _concertRepository.DeleteAsync(request.Id);

        return new CustomOperationReply
        {
            Code = 0,
            Message = ""
        };
    }

    public override async Task<CustomOperationReply> UpdateConcert(Concert request, ServerCallContext? context)
    {
        var validator = new ConcertValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return new CustomOperationReply
            {
                Code = 3,
                Message = validation.Errors.ToString()
            };
        }

        await _concertRepository.UpdateAsync(request);

        return new CustomOperationReply
        {
            Code = 0,
        };
    }

    public override Task<GetAllConcertsReply> GetAllConcerts(Empty request, ServerCallContext? context)
    {
        var concerts = _concertRepository.GetAll();
        var result = new GetAllConcertsReply();
        result.Concerts.AddRange(concerts);
        return Task.FromResult(result);
    }

    public override async Task<ConcertReply> GetById(ConcertId request, ServerCallContext? context)
    {
        var validator = new ConcertIdValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return new ConcertReply
            {
                Concert = null
            };
        }

        var concert = _concertRepository.GetById(request.Id);
        return await Task.FromResult(concert);
    }

    public override async Task<CustomOperationReply> DeleteConcert(ConcertId request, ServerCallContext? context)
    {
        var validator = new ConcertIdValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return new CustomOperationReply
            {
                Code = 3,
                Message = validation.Errors.ToString()
            };
        }

        await _bookingRepository.DeleteByConcertId(request.Id);
        await _concertRepository.DeleteAsync(request.Id);
        return new CustomOperationReply
        {
            Code = 0,
            Message = ""
        };
    }
}