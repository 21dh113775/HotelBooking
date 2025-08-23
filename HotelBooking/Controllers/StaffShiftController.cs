using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.DTOs;
using FluentValidation;
using HotelBooking.Validators;
using Microsoft.AspNetCore.Http;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/staffshift")]
    [Authorize(Roles = "Admin")]
    public class StaffShiftController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<StaffShiftCreateDto> _validator;

        public StaffShiftController(AppDbContext context, IValidator<StaffShiftCreateDto> validator)
        {
            _context = context;
            _validator = validator;
        }

        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetShifts(int staffId)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}");
                var shifts = await _context.StaffShifts
                    .Where(s => s.StaffId == staffId)
                    .Select(s => new
                    {
                        s.Id,
                        s.StaffId,
                        s.ShiftDate,
                        s.ShiftTime
                    })
                    .ToListAsync();
                return Ok(shifts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách ca làm việc.", error = ex.Message });
            }
        }

        [HttpGet("by-date")]
        public async Task<IActionResult> GetShiftsByDate([FromQuery] DateTime? date, [FromQuery] int? staffId)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}, Date: ${date}, StaffId: ${staffId}");
                var query = _context.StaffShifts.AsQueryable();
                if (date.HasValue) query = query.Where(s => s.ShiftDate.Date == date.Value.Date);
                if (staffId.HasValue) query = query.Where(s => s.StaffId == staffId.Value);

                var shifts = await query
                    .Select(s => new
                    {
                        s.Id,
                        s.StaffId,
                        s.ShiftDate,
                        s.ShiftTime
                    })
                    .ToListAsync();
                return Ok(shifts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy ca làm việc theo ngày.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignShift([FromBody] StaffShiftCreateDto dto)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}");
                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return BadRequest(new { message = validationResult.Errors.First().ErrorMessage });

                var staff = await _context.Users.FindAsync(dto.StaffId);
                if (staff == null || staff.RoleId != 3)
                    return BadRequest(new { message = "StaffId không hợp lệ hoặc không phải Staff." });

                var dailyShifts = await _context.StaffShifts
                    .CountAsync(s => s.StaffId == dto.StaffId && s.ShiftDate.Date == dto.ShiftDate.Date);
                if (dailyShifts >= 2)
                    return BadRequest(new { message = "Staff đã đạt giới hạn 2 ca/ngày." });

                if (await _context.StaffShifts.AnyAsync(s => s.StaffId == dto.StaffId &&
                    s.ShiftDate == dto.ShiftDate && s.ShiftTime == dto.ShiftTime))
                    return BadRequest(new { message = "Ca làm việc đã tồn tại cho ngày này." });

                var shift = new StaffShift
                {
                    StaffId = dto.StaffId,
                    ShiftDate = dto.ShiftDate,
                    ShiftTime = dto.ShiftTime
                };
                _context.StaffShifts.Add(shift);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Gán ca thành công.", id = shift.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi gán ca làm việc.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShift(int id, [FromBody] StaffShiftCreateDto dto)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}");
                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                    return BadRequest(new { message = validationResult.Errors.First().ErrorMessage });

                var shift = await _context.StaffShifts.FindAsync(id);
                if (shift == null) return NotFound(new { message = "Ca làm việc không tìm thấy." });
                if (shift.ShiftDate <= DateTime.UtcNow)
                    return BadRequest(new { message = "Không thể sửa ca đã bắt đầu hoặc quá khứ." });

                var staff = await _context.Users.FindAsync(dto.StaffId);
                if (staff == null || staff.RoleId != 3)
                    return BadRequest(new { message = "StaffId không hợp lệ hoặc không phải Staff." });

                if (await _context.StaffShifts.AnyAsync(s => s.Id != id && s.StaffId == dto.StaffId &&
                    s.ShiftDate == dto.ShiftDate && s.ShiftTime == dto.ShiftTime))
                    return BadRequest(new { message = "Ca làm việc đã tồn tại cho ngày này." });

                shift.StaffId = dto.StaffId;
                shift.ShiftDate = dto.ShiftDate;
                shift.ShiftTime = dto.ShiftTime;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật ca thành công.", id = shift.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật ca làm việc.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}");
                var shift = await _context.StaffShifts.FindAsync(id);
                if (shift == null) return NotFound(new { message = "Ca làm việc không tìm thấy." });
                if (shift.ShiftDate <= DateTime.UtcNow)
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