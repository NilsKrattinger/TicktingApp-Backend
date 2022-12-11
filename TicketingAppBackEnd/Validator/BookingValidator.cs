using FluentValidation;
using TicketingAppBackEnd.Protos;

namespace TicketingAppBackEnd.Validator;

public class BookingValidator:AbstractValidator<Booking>
{
    public BookingValidator()
    {
        RuleFor(x => x.ConcertId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PaymentType).NotEmpty().Length(1, 25);
        RuleFor(x => x.DateUTC).NotEmpty();
    }
}
