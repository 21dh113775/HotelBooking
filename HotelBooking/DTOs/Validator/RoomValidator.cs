namespace HotelBooking.DTOs.Validator
{
    using FluentValidation;
    using HotelBooking.DTOs.Rom;

    public class RoomCreateDtoValidator : AbstractValidator<RoomCreateDto>
    {
        public RoomCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.PricePerHour).GreaterThanOrEqualTo(0);
        }
    }

}
