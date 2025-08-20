using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherController : ControllerBase
    {
        private readonly AppDbContext _context;
        public VoucherController(AppDbContext context) => _context = context;

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyVoucher(string code)
        {
            var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == code && v.IsActive && v.ExpiryDate >= DateTime.UtcNow);
            if (voucher == null)
                return NotFound(new { message = "Mã không hợp lệ hoặc đã hết hạn" });

            return Ok(voucher);
        }
    }

}
