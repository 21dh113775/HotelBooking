using FluentValidation;
using HotelBooking.DTOs;

namespace HotelBooking.Validators
{
    public class VoucherCreateDtoValidator : AbstractValidator<VoucherCreateDto>
    {
        public VoucherCreateDtoValidator()
        {
            // === GIẢI THÍCH: Kiểm tra mã voucher không rỗng và duy nhất ===
            // Lý do: Ngăn chặn tạo mã trùng lặp hoặc không hợp lệ ===
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã voucher là bắt buộc.")
                .MaximumLength(20).WithMessage("Mã voucher không vượt quá 20 ký tự.");

            // === GIẢI THÍCH: Kiểm tra phần trăm giảm giá hợp lý ===
            // Lý do: Đảm bảo giảm giá trong khoảng 1-100% ===
            RuleFor(x => x.Discount)
                .GreaterThan(0).WithMessage("Giảm giá phải lớn hơn 0%.")
                .LessThanOrEqualTo(100).WithMessage("Giảm giá không vượt quá 100%.");

            // === GIẢI THÍCH: Kiểm tra ngày hết hạn phải sau hiện tại ===
            // Lý do: Tránh tạo voucher đã hết hạn ngay từ đầu ===
            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Ngày hết hạn phải sau ngày hiện tại.");
        }
    }
}