using TicketingApp.Protos;

namespace TicketingApp.Models.Views;

public class BookViewData
{
    public BookViewData(Booking booking, Concert concert)
    {
        Booking = booking;
        Concert = concert;
    }

    public Booking Booking { get; }

    public Concert Concert { get; }
}