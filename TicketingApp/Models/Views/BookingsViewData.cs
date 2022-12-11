using TicketingApp.Protos;

namespace TicketingApp.Models.Views;

public class BookingsViewData
{
    public Dictionary<int, string> ConcertId { get; }
    public List<Booking> Bookings { get; }

    public BookingsViewData(Dictionary<int, string> concertId, List<Booking> bookings)
    {
        ConcertId = concertId;
        Bookings = bookings;
    }
}