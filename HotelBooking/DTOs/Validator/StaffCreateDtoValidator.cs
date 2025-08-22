using FluentValidation;
using HotelBooking.DTOs;

namespace HotelBooking.Validators
{
    public class StaffShiftCreateDtoValidator : AbstractValidator<StaffShiftCreateDto>
    {
        public StaffShiftCreateDtoValidator()
        {
            // === GIẢI THÍCH: Kiểm tra StaffId hợp lệ ===
            // Lý do: Đảm bảo staff tồn tại ===
            RuleFor(x => x.StaffId)
                .GreaterThan(0).WithMessage("StaffId phải lớn hơn 0.");

            // === GIẢI THÍCH: Kiểm tra ngày ca làm việc ===
            // Lý do: Ngày phải sau hiện tại ===
            RuleFor(x => x.ShiftDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Ngày ca làm phải sau ngày hiện tại.");

            // === GIẢI THÍCH: Kiểm tra thời gian ca ===
            // Lý do: Chỉ chấp nhận các giá trị hợp lệ ===
            RuleFor(x => x.ShiftTime)
                .Must(time => time == "Morning" || time == "Afternoon" || time == "Night")
                .WithMessage("ShiftTime phải là 'Morning', 'Afternoon' hoặc 'Night'.");
        }
    }
}