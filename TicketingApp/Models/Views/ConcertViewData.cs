using TicketingApp.Protos;

namespace TicketingApp.Models.Views;

public class ConcertViewData
{
    public ConcertId ConcertId { get; }
    public List<Concert> Concerts { get; }

    public ConcertViewData(ConcertId concertId, List<Concert> concerts)
    {
        ConcertId = concertId;
        Concerts = concerts;
    }
}