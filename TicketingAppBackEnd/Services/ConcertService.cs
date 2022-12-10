using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TicketingAppBackEnd.Sql.Interfaces;
using TicketingAppBackEnd.Protos;
namespace TicketingAppBackEnd.Services
{
    public class ConcertService : TicketingAppBackEnd.Protos.ConcertService.ConcertServiceBase
    {
        private readonly IConcertRepository _concertRepository;

        public ConcertService(IConcertRepository concertRepository)
        {
            _concertRepository = concertRepository;
        }

        public override async Task<CustomOperationReply> AddConcert(Concert request, ServerCallContext context)
        {
            var concert = new Concert
            {
                Id = request.Id,
                Artist = request.Artist, 
                Price = request.Price,
                Place = request.Place
            };
            await _concertRepository.AddAsync(concert);

            return new CustomOperationReply()
            {
                Code = 0,
                Message = concert.Id.ToString()
            };
        }

        public async Task<CustomOperationReply> DeleteConcert(Concert request, ServerCallContext context)
        {
            await _concertRepository.DeleteAsync(request.Id);

            return new CustomOperationReply()
            {
                Code = 0,
                Message = ""
            };
        }

        public override async Task<CustomOperationReply> UpdateConcert(Concert request, ServerCallContext context)
        {
            var concert = new Concert
            {
                Id = request.Id,
                Artist = request.Artist,
                Price = request.Price,
                Place = request.Place
            };
            await _concertRepository.UpdateAsync(concert);

            return new CustomOperationReply()
            {
                Code = 0,
            };
        }

        public override Task<GetAllConcertsReply> GetAllConcerts(Empty request, ServerCallContext context)
        {
            var concerts = _concertRepository.GetAll();
            /* .Select(x => new TicketingAppBackEnd.Protos.Concert
          {
               Id = x.Id,
               Artist = x.Artist,
               Price = x.Price,
               Place = x.Place
           });*/


            //var result = new GetAllConcertsReply();
            var result = new GetAllConcertsReply();
            result.Concerts.AddRange(concerts);


            return Task.FromResult(result);
        }
    }
}
