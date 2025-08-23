using FluentValidation;
using HotelBooking.DTOs;

namespace HotelBooking.Validators
{
    public class StaffShiftCreateDtoValidator : AbstractValidator<StaffShiftCreateDto>
    {
        public StaffShiftCreateDtoValidator()
        {
            RuleFor(x => x.StaffId)
                .GreaterThan(0).WithMessage("StaffId phải lớn hơn 0.");
            RuleFor(x => x.ShiftDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Ngày ca làm phải sau ngày hiện tại.");
            RuleFor(x => x.ShiftTime)
                .Must(time => time == "Morning" || time == "Afternoon" || time == "Night")
                .WithMessage("ShiftTime phải là 'Morning', 'Afternoon' hoặc 'Night'.");
        }
    }
}