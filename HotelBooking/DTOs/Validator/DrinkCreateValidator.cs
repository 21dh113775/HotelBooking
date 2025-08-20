using FluentValidation;
using HotelBooking.DTOs.Drink;

namespace HotelBooking.DTOs.Validator
{
    public class DrinkCreateValidator : AbstractValidator<DrinkCreateDto>
    {
        public DrinkCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }

}
