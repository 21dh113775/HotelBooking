using FluentValidation;
using HotelBooking.DTOs.Booking;

namespace HotelBooking.DTOs.Validator
{
    public class BookingCreateValidator : AbstractValidator<BookingCreateDto>
    {
        public BookingCreateValidator()
        {
            RuleFor(x => x.RoomId).GreaterThan(0);
            RuleFor(x => x.StartTime)
    .LessThan(x => x.EndTime)
    .WithMessage("StartTime must be before EndTime");
        }
    }

}
