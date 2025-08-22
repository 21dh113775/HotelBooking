using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.DTOs;
using FluentValidation.AspNetCore;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VoucherController(AppDbContext context) => _context = context;

        // === GIẢI THÍCH: Endpoint GET danh sách voucher ===
        // Lý do: Cho phép user/admin lấy list voucher để hiển thị trên frontend ===
        [HttpGet]
        [Authorize] // Yêu cầu đăng nhập
        public async Task<IActionResult> GetVouchers()
        {
            var vouchers = await _context.Vouchers.ToListAsync();
            return Ok(vouchers);
        }

        // === GIẢI THÍCH: Endpoint POST tạo voucher mới (chỉ admin) ===
        // Lý do: Cho phép admin tạo mã giảm giá, với validation ===
        // === KHÔI PHỤC BẢO MẬT: Uncomment [Authorize(Roles = "Admin")] để yêu cầu token JWT với vai trò Admin ===
        // Lý do lỗi trước: Khi comment out, endpoint mở cho tất cả, nhưng giờ khôi phục để tránh rủi ro bảo mật (chỉ admin truy cập) ===
        [HttpPost]
        [Authorize(Roles = "Admin")] // Uncomment để khôi phục bảo mật sau test
        public async Task<IActionResult> CreateVoucher([FromBody] VoucherCreateDto dto)
        {
            try
            {
                // Kiểm tra mã đã tồn tại
                if (await _context.Vouchers.AnyAsync(v => v.Code == dto.Code))
                    return BadRequest(new { message = "Mã voucher đã tồn tại." });

                var voucher = new Voucher
                {
                    Code = dto.Code,
                    Discount = dto.Discount,
                    ExpiryDate = dto.ExpiryDate,
                    IsActive = dto.IsActive
                };

                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();
                return Ok(voucher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo voucher.", error = ex.Message });
            }
        }

        // === GIẢI THÍCH: Endpoint POST apply voucher (cập nhật từ mã cũ) ===
        // Lý do: Kiểm tra và trả về chi tiết voucher để frontend áp dụng giảm giá ===
        [HttpPost("apply")]
        [Authorize] // Yêu cầu đăng nhập
        public async Task<IActionResult> ApplyVoucher(string code)
        {
            var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == code && v.IsActive && v.ExpiryDate >= DateTime.UtcNow);
            if (voucher == null)
                return NotFound(new { message = "Mã không hợp lệ hoặc đã hết hạn" });
            return Ok(voucher); // Trả về object voucher để lấy Discount
        }
    }
}