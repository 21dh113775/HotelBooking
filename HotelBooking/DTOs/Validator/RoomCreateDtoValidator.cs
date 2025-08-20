using FluentValidation;
using HotelBooking.DTOs;

namespace HotelBooking.Validators
{
    public class RoomCreateDtoValidator : AbstractValidator<RoomCreateDto>
    {
        public RoomCreateDtoValidator()
        {
            RuleFor(x => x.RoomNumber)
                .NotEmpty().WithMessage("RoomNumber is required.")
                .MaximumLength(50).WithMessage("RoomNumber must not exceed 50 characters.");
            RuleFor(x => x.PricePerNight)
                .GreaterThan(0).WithMessage("PricePerNight must be greater than 0.");
            RuleFor(x => x.Image)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024).WithMessage("Image size must not exceed 5MB.")
                .Must(file => file == null || new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("Image must be JPG or PNG.");
        }
    }
}