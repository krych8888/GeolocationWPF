using Entities.Dtos.Requests;
using FluentValidation;
using System.Net;

namespace EventBookingApi.Validation;

public class CreateEventRequestValidator : AbstractValidator<GetGeolocationRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(request => request.IpAddress).NotEmpty();
        RuleFor(request => request.IpAddress).Must(x => IPAddress.TryParse(x, out IPAddress? result));
    }
}
