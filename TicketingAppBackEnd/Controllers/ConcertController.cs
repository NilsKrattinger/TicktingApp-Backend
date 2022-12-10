using Microsoft.AspNetCore.Mvc;
using TicketingAppBackEnd.Protos;
using TicketingAppBackEnd.Sql.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketingAppBackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConcertController : ControllerBase
{
    private readonly IConcertRepository _concertRepository;

    public ConcertController(IConcertRepository concertRepository)
    {
        _concertRepository = concertRepository;
    }

    // GET: api/<ConcertController>
    [HttpGet]
    public List<Concert> Get()
    {
        return _concertRepository.GetAll();
    }

    // GET api/<ConcertController>/5
    [HttpGet("{id}")]
    public ConcertReply Get(int id)
    {
        return _concertRepository.GetById(id);
    }

    // POST api/<ConcertController>
    [HttpPost]
    public async Task PostAsync([FromBody] Concert concert)
    {
        await _concertRepository.AddAsync(concert);
    }

    // PUT api/<ConcertController>/5
    [HttpPut("{id}")]
    public async Task PutAsync(Concert concert, string id)
    {
        await _concertRepository.UpdateAsync(concert);
    }

    // DELETE api/<ConcertController>/5
    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        await _concertRepository.DeleteAsync(id);
    }
}