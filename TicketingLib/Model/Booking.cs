namespace TicketingLib.Model;

public class Booking
{
    public int Id { get; set; }
    public int Price { get; set; }
    public DateTime Date { get; set; }
    public string Email { get; set; }
    public string RIB { get; set; }
    public int ConcertId { get; set; }
    public Concert Concert { get; set; }
}