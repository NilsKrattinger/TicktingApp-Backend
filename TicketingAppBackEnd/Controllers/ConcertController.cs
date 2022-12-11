using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using TicketingAppBackEnd.Interfaces;
using TicketingAppBackEnd.Protos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketingAppBackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConcertController : ControllerBase
{
    private readonly IConcertServices _concertServices;

    public ConcertController(IConcertServices concertServices)
    {
        _concertServices = concertServices;
    }

    // GET: api/<ConcertController>
    [HttpGet]
    public async Task<List<Concert>> Get()
    {
        return (await _concertServices.GetAllConcerts(new Empty())).Concerts.ToList();
    }

    // GET api/<ConcertController>/5
    [HttpGet("{id}")]
    public async Task<ConcertReply> Get(int id)
    {
        return await _concertServices.GetById(new ConcertId { Id = id });
    }

    // POST api/<ConcertController>
    [HttpPost]
    public async Task PostAsync([FromBody] Concert concert)
    {
        await _concertServices.AddConcert(concert);
    }

    // PUT api/<ConcertController>/5
    [HttpPut("{id}")]
    public async Task PutAsync(Concert concert, string id)
    {
        await _concertServices.UpdateConcert(concert);
    }

    // DELETE api/<ConcertController>/5
    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        await _concertServices.DeleteConcert(new ConcertId { Id = id });
    }
}