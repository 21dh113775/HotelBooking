using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.DTOs;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/staffshift")]
    [Authorize(Roles = "Admin")] // === GIẢI THÍCH: Bảo vệ endpoint chỉ cho Admin ===
    // Lý do: Đảm bảo chỉ admin gán/xóa ca làm việc ===
    public class StaffShiftController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StaffShiftController(AppDbContext context) => _context = context;

        // === GIẢI THÍCH: Endpoint GET lấy ca làm việc của staff ===
        // Lý do: Admin xem lịch ca để quản lý, phù hợp logic thực tế (lập kế hoạch nhân sự) ===
        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetShifts(int staffId)
        {
            var shifts = await _context.StaffShifts.Where(s => s.StaffId == staffId).ToListAsync();
            return Ok(shifts);
        }

        // === GIẢI THÍCH: Endpoint POST gán ca làm việc mới ===
        // Lý do: Admin thêm ca, với kiểm tra staff tồn tại và ca chưa trùng ===
        [HttpPost]
        public async Task<IActionResult> AssignShift([FromBody] StaffShiftCreateDto dto)
        {
            try
            {
                var staff = await _context.Users.FindAsync(dto.StaffId);
                if (staff == null || staff.RoleId != 3) // Giả sử RoleId=3 là Staff
                    return BadRequest(new { message = "StaffId không hợp lệ." });

                // Kiểm tra logic thực tế: Không gán ca trùng ngày/giờ
                if (await _context.StaffShifts.AnyAsync(s => s.StaffId == dto.StaffId && s.ShiftDate == dto.ShiftDate && s.ShiftTime == dto.ShiftTime))
                    return BadRequest(new { message = "Ca làm việc đã tồn tại cho ngày này." });

                var shift = new StaffShift
                {
                    StaffId = dto.StaffId,
                    ShiftDate = dto.ShiftDate,
                    ShiftTime = dto.ShiftTime
                };

                _context.StaffShifts.Add(shift);
                await _context.SaveChangesAsync();
                return Ok(shift);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi gán ca làm việc.", error = ex.Message });
            }
        }

        // === GIẢI THÍCH: Endpoint DELETE xóa ca làm việc ===
        // Lý do: Admin xóa ca cũ, nhưng chỉ nếu ca chưa bắt đầu (logic thực tế: tránh xóa lịch đang diễn ra) ===
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            try
            {
                var shift = await _context.StaffShifts.FindAsync(id);
                if (shift == null) return NotFound(new { message = "Ca làm việc không tìm thấy." });

                if (shift.ShiftDate <= DateTime.UtcNow) // Logic thực tế: Không xóa ca đã bắt đầu
                    return BadRequest(new { message = "Không thể xóa ca làm việc đã bắt đầu hoặc quá khứ." });

                _context.StaffShifts.Remove(shift);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa ca làm việc thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa ca làm việc.", error = ex.Message });
            }
        }
    }
}