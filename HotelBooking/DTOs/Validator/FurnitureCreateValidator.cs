using FluentValidation;
using HotelBooking.DTOs.Furniture;

namespace HotelBooking.DTOs.Validator
{
    public class FurnitureCreateValidator : AbstractValidator<FurnitureCreateDto>
    {
        public FurnitureCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }

}
