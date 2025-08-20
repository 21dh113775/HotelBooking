using FluentValidation;
using HotelBooking.DTOs.Booking;

namespace HotelBooking.DTOs.Validator
{
    public class BookingCreateValidator : AbstractValidator<BookingCreateDto>
    {
        public BookingCreateValidator()
        {
            RuleFor(x => x.RoomId)
                .GreaterThan(0).WithMessage("RoomId must be greater than 0.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("StartTime is required.")
                .LessThan(x => x.EndTime).WithMessage("StartTime must be before EndTime.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("EndTime is required.")
                .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.");

            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required.");

            RuleFor(x => x.DrinkIds)
                .Must(ids => ids == null || ids.All(id => id > 0)).WithMessage("All DrinkIds must be greater than 0.");
        }
    }
}