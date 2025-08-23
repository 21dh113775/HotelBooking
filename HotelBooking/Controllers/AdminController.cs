using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.DTOs;
using AutoMapper;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using HotelBooking.DTOs.Room;
using HotelBooking.Modules.Entity_Models;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        RoleName = u.Role.Name
                    })
                    .ToListAsync();
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 128,
                    WriteIndented = true
                };
                var jsonString = JsonSerializer.Serialize(users, options);
                Console.WriteLine($"GetUsers response body: ${jsonString}");
                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUsers error: ${ex.Message}");
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách user.", error = ex.Message });
            }
        }

        [HttpPut("users/update-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] AdminUserUpdateDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                if (user == null) return NotFound(new { message = "User không tìm thấy." });
                var role = await _context.Roles.FindAsync(dto.RoleId);
                if (role == null) return BadRequest(new { message = "Role không hợp lệ." });
                user.RoleId = dto.RoleId;
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật role.", error = ex.Message });
            }
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                var rooms = await _context.Rooms.ToListAsync();
                var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, MaxDepth = 128, WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(rooms, options);
                Console.WriteLine($"GetRooms response body: ${jsonString}");
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách phòng.", error = ex.Message });
            }
        }

        [HttpPost("rooms")]
        public async Task<IActionResult> CreateRoom([FromForm] RoomCreateDto dto)
        {
            try
            {
                if (await _context.Rooms.AnyAsync(r => r.RoomNumber == dto.RoomNumber))
                    return BadRequest(new { message = $"Số phòng {dto.RoomNumber} đã tồn tại." });
                var room = new Room
                {
                    RoomNumber = dto.RoomNumber,
                    PricePerNight = dto.PricePerNight,
                    IsAvailable = dto.IsAvailable,
                    Description = dto.Description,
                    ImageUrl = dto.Image != null ? "/Uploads/" + Guid.NewGuid() + Path.GetExtension(dto.Image.FileName) : null
                };
                if (dto.Image != null)
                {
                    var uploadsDir = Path.Combine(_env.WebRootPath ?? Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                    Directory.CreateDirectory(uploadsDir);
                    var imageName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                    var path = Path.Combine(uploadsDir, imageName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await dto.Image.CopyToAsync(stream);
                    room.ImageUrl = "/Uploads/" + imageName;
                }
                room.CreatedAt = DateTime.UtcNow;
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo phòng.", error = ex.Message });
            }
        }

        [HttpPut("rooms/{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromForm] RoomUpdateDto dto)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);
                if (room == null) return NotFound(new { message = "Phòng không tìm thấy." });

                room.RoomNumber = dto.RoomNumber;
                room.PricePerNight = dto.PricePerNight;
                room.IsAvailable = dto.IsAvailable;
                room.Description = dto.Description;

                if (dto.Image != null)
                {
                    var uploadsDir = Path.Combine(_env.WebRootPath ?? Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                    Directory.CreateDirectory(uploadsDir);
                    var imageName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                    var path = Path.Combine(uploadsDir, imageName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await dto.Image.CopyToAsync(stream);
                    room.ImageUrl = "/Uploads/" + imageName;
                }

                await _context.SaveChangesAsync();
                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật phòng.", error = ex.Message });
            }
        }

        [HttpDelete("rooms/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);
                if (room == null) return NotFound(new { message = "Phòng không tìm thấy." });
                if (await _context.Bookings.AnyAsync(b => b.RoomId == id && (b.Status == "Pending" || b.Status == "Confirmed")))
                    return BadRequest(new { message = "Không thể xóa phòng đang có booking hoạt động." });
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa phòng thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa phòng.", error = ex.Message });
            }
        }

        [HttpGet("vouchers")]
        public async Task<IActionResult> GetVouchers()
        {
            try
            {
                var vouchers = await _context.Vouchers.ToListAsync();
                var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, MaxDepth = 128, WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(vouchers, options);
                Console.WriteLine($"GetVouchers response body: ${jsonString}");
                return Ok(vouchers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách voucher.", error = ex.Message });
            }
        }

        [HttpPut("vouchers/{id}")]
        public async Task<IActionResult> UpdateVoucher(int id, [FromBody] VoucherCreateDto dto)
        {
            try
            {
                var voucher = await _context.Vouchers.FindAsync(id);
                if (voucher == null) return NotFound(new { message = "Voucher không tìm thấy." });
                voucher.Code = dto.Code;
                voucher.Discount = dto.Discount;
                voucher.ExpiryDate = dto.ExpiryDate;
                voucher.IsActive = dto.IsActive;
                await _context.SaveChangesAsync();
                return Ok(voucher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật voucher.", error = ex.Message });
            }
        }

        [HttpDelete("vouchers/{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            try
            {
                var voucher = await _context.Vouchers.FindAsync(id);
                if (voucher == null) return NotFound(new { message = "Voucher không tìm thấy." });
                _context.Vouchers.Remove(voucher);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa voucher thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa voucher.", error = ex.Message });
            }
        }

        [HttpPost("vouchers")]
        public async Task<IActionResult> CreateVoucher([FromBody] VoucherCreateDto dto)
        {
            try
            {
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

        [HttpGet("staff")]
        public async Task<IActionResult> GetStaffs()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}");
                var staffs = await _context.Users
                    .Where(u => u.RoleId == 3) // Giả sử RoleId=3 là Staff
                    .Select(u => new
                    {
                        u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber
                    })
                    .ToListAsync();
                return Ok(staffs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách nhân viên.", error = ex.Message });
            }
        }

        [HttpPost("staff")]
        public async Task<IActionResult> CreateStaff([FromBody] StaffCreateDto dto)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}, Body: ${JsonSerializer.Serialize(dto)}");
                if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                    return BadRequest(new { message = "Email đã tồn tại." });

                var user = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    RoleId = 3 // Staff role
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Tạo staff thành công.", id = user.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo staff.", error = ex.Message });
            }
        }

        [HttpPut("staff/{id}")]
        public async Task<IActionResult> UpdateStaff(int id, [FromBody] StaffCreateDto dto)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}, Body: ${JsonSerializer.Serialize(dto)}");
                var user = await _context.Users.FindAsync(id);
                if (user == null) return NotFound(new { message = "Staff không tìm thấy." });
                if (user.RoleId != 3) return BadRequest(new { message = "Không phải staff." });
                if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                    return BadRequest(new { message = "Email đã tồn tại." });

                user.FullName = dto.FullName;
                user.Email = dto.Email;
                user.PhoneNumber = dto.PhoneNumber;
                if (!string.IsNullOrEmpty(dto.Password))
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật staff thành công.", id = user.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật staff.", error = ex.Message });
            }
        }

        [HttpDelete("staff/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received token: ${authHeader}");
                var user = await _context.Users.FindAsync(id);
                if (user == null) return NotFound(new { message = "Staff không tìm thấy." });
                if (user.RoleId != 3) return BadRequest(new { message = "Không phải staff." });
                if (await _context.StaffShifts.AnyAsync(s => s.StaffId == id))
                    return BadRequest(new { message = "Không thể xóa staff có ca làm việc." });
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa staff thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa staff.", error = ex.Message });
            }
        }

        [HttpGet("users/raw")]
        public IActionResult GetUsersRaw()
        {
            var users = _context.Users.Include(u => u.Role).ToList();
            return Ok(users);
        }

    }
}