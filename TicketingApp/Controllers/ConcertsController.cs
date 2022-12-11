using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.Models;
using TicketingApp.Models.Views;
using TicketingApp.Protos;

namespace TicketingApp.Controllers;

public class ConcertsController : Controller
{
    private readonly ConcertService.ConcertServiceClient _concertServiceClient;
    private readonly BookingService.BookingServiceClient _bookingServiceClient;


    public ConcertsController(ILogger<ConcertsController> logger,
        ConcertService.ConcertServiceClient concertServiceClient,
        BookingService.BookingServiceClient bookingServiceClient)
    {
        _concertServiceClient = concertServiceClient;
        _bookingServiceClient = bookingServiceClient;
    }

    public Task<IActionResult> Index()
    {
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Concerts)));
    }

    [HttpGet]
    public IActionResult Concerts()
    {
        var concerts = _concertServiceClient.GetAllConcerts(new Empty()).Concerts.ToList();
        var dataBag = new ConcertViewData(new ConcertId(), concerts);
        return View(dataBag);
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> Update(int concertId)
    {
        var request = new ConcertId
        {
            Id = concertId
        };
        var concert = _concertServiceClient.GetById(request);
        if (concert.Concert == null) return await Task.FromResult<IActionResult>(RedirectToAction(nameof(Error)));
        return View(concert.Concert);
    }

    [HttpPost]
    public Task<IActionResult> Delete(ConcertId concertId)
    {
        _concertServiceClient.DeleteConcert(concertId);
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Concerts)));
    }

    [HttpPost]
    public Task<IActionResult> Create(Concert concert)
    {
        _concertServiceClient.AddConcert(concert);
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Concerts)));
    }

    [HttpPost]
    public Task<IActionResult> Update(Concert concert)
    {
        _concertServiceClient.UpdateConcert(concert);
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Concerts)));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}