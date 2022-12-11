using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using TicketingAppBackEnd.Interfaces;
using TicketingAppBackEnd.Protos;


namespace TicketingAppBackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookingController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<List<Booking>> Get()
    {
        var reply = await _bookService.GetAllBookings(new Empty());
        return reply.Booking.ToList();
    }

    [HttpPost]
    public async Task PostAsync([FromBody] Booking reservation)
    {
        var res = await _bookService.AddBooking(reservation);
    }
}