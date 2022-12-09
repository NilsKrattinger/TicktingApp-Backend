using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.Models;
using TicketingApp.Protos;

namespace TicketingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConcertService.ConcertServiceClient _concertServiceClient;
        private readonly BookingService.BookingServiceClient _bookingServiceClient;


        public HomeController(ILogger<HomeController> logger, ConcertService.ConcertServiceClient concertServiceClient )
        {
            _logger = logger;
            _concertServiceClient = concertServiceClient;
        }

        public IActionResult Index()
        {
            var concerts = (_concertServiceClient.GetAllConcerts(new Empty()).Concerts)
               .Select(x => new Concert
               {
                   Id = x.Id,
                   Artist = x.Artist,
                   Price = x.Price,
                   Place = x.Place
               }).ToList();
            return View(concerts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Book(int concertId)
        {
            var booking = new Booking()
            {
                ConcertId = concertId,
            };
            return View(booking);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Booking(Booking booking)
        {
            await _bookingServiceClient.AddBooking(booking);
            return RedirectToAction(nameof(Index));
        }
    }
}
