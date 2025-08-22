using FluentValidation;
using HotelBooking.DTOs;

namespace HotelBooking.Validators
{
    public class AdminUserUpdateDtoValidator : AbstractValidator<AdminUserUpdateDto>
    {
        public AdminUserUpdateDtoValidator()
        {
            // === GIẢI THÍCH: Kiểm tra UserId hợp lệ ===
            // Lý do: Đảm bảo UserId tồn tại và lớn hơn 0 ===
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId phải lớn hơn 0.");

            // === GIẢI THÍCH: Kiểm tra RoleId hợp lệ ===
            // Lý do: Vai trò phải tồn tại trong hệ thống (1-4 dựa trên seeding) ===
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId phải lớn hơn 0.");
        }
    }
}