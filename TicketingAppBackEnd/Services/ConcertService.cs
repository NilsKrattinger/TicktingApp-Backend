using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Sql.Interfaces;

namespace TicketingAppBackEnd.Services
{
    public class ConcertService : TicketingAppBackEnd.Protos.ConcertService.ConcertServiceBase
    {
        private readonly IConcertRepository _concertRepository;

        public ConcertService(IConcertRepository concertRepository)
        {
            _concertRepository = concertRepository;
        }

        public async override Task<ConcertReply> AddConcert(ConcertRequest request, ServerCallContext context)
        {
            var concert = new TicketingLib.Model.Concert
            {
                Id = request.Id,
                Artist = request.Artist, 
                Price = request.Price,
                Place = request.Place
            };
            await _concertRepository.AddAsync(concert);

            return new ConcertReply
            {
                Id = concert.Id
            };
        }

        public async override Task<ConcertReply> DeleteConcert(DeleteConcertRequest request, ServerCallContext context)
        {
            await _concertRepository.DeleteAsync(request.Id);

            return new ConcertReply
            {
                Id = request.Id
            };
        }

        public async override Task<ConcertReply> UpdateConcert(ConcertRequest request, ServerCallContext context)
        {
            var concert = new TicketingLib.Model.Concert
            {
                Id = request.Id,
                Artist = request.Artist,
                Price = request.Price,
                Place = request.Place
            };
            await _concertRepository.UpdateAsync(concert);

            return new ConcertReply
            {
                Id = concert.Id
            };
        }

        public override Task<GetAllConcertsReply> GetAllConcerts(Empty request, ServerCallContext context)
        {
            var concerts = (_concertRepository.GetAll())
               .Select(x => new TicketingAppBackEnd.Protos.Concert
               {
                   Id = x.Id,
                   Artist = x.Artist,
                   Price = x.Price,
                   Place = x.Place
               });

            var result = new GetAllConcertsReply();
            result.Concerts.AddRange(concerts);

            return Task.FromResult(result);
        }
    }
}
