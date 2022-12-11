using FluentValidation;
using TicketingAppBackEnd.Protos;

namespace TicketingAppBackEnd.Validator;

public class ConcertValidator : AbstractValidator<Concert>
{
    public ConcertValidator()
    {
        RuleFor(x => x.Artist).NotEmpty().Length(4, 20);
        RuleFor(x => x.Place).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
    }
}

public class ConcertIdValidator : AbstractValidator<ConcertId>
{
    public ConcertIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}