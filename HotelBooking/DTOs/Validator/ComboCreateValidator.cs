using FluentValidation;
using HotelBooking.DTOs.Combo;

namespace HotelBooking.DTOs.Validator
{
    public class ComboCreateValidator : AbstractValidator<ComboCreateDto>
    {
        public ComboCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.FurnitureIds).NotEmpty();
        }
    }

}
