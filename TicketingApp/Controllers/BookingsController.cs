using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.Models;
using TicketingApp.Models.Views;
using TicketingApp.Protos;

namespace TicketingApp.Controllers;

public class BookingsController : Controller
{
    private readonly ConcertService.ConcertServiceClient _concertServiceClient;
    private readonly BookingService.BookingServiceClient _bookingServiceClient;


    public BookingsController(ILogger<BookingsController> logger,
        ConcertService.ConcertServiceClient concertServiceClient,
        BookingService.BookingServiceClient bookingServiceClient)
    {
        _concertServiceClient = concertServiceClient;
        _bookingServiceClient = bookingServiceClient;
    }

    [HttpGet]
    public async Task<IActionResult> Book(int concertId)
    {
        var request = new ConcertId
        {
            Id = concertId
        };
        var concert = _concertServiceClient.GetById(request);
        if (concert.Concert == null)
        {
            return await Task.FromResult<IActionResult>(RedirectToAction(nameof(Error)));
        }

        var dataBag = new BookViewData(new Booking(), concert.Concert);
        return View(dataBag);
    }

    [HttpPost]
    public Task<IActionResult> Booking(Booking booking)
    {
        _bookingServiceClient.AddBooking(booking);
        return Task.FromResult<IActionResult>(Redirect("/"));
    }

    [HttpGet]
    public IActionResult Bookings()
    {
        var bookings = _bookingServiceClient.GetAllBookings(new Empty()).Booking.ToList();
        var concertId = new Dictionary<int,string>();
        foreach (var booking in bookings)
        {
            if (concertId.ContainsKey(booking.ConcertId)) continue;
            var concert = _concertServiceClient.GetById(new ConcertId { Id = booking.ConcertId });
            concertId.Add(booking.Id,concert.Concert.Artist);
        }

        var dataBag = new BookingsViewData(concertId, bookings);
        return View(dataBag);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}