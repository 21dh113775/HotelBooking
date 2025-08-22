using HotelBooking.Domain.Entities;
using HotelBooking.DTOs.Booking;
using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BookingController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // === GIẢI THÍCH: Phương thức GET để lấy danh sách tất cả các booking ===
        // Lý do: Cho phép người dùng hoặc admin xem toàn bộ lịch sử đặt phòng, bao gồm quan hệ với room, user, combo và drinks ===
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.Room)
                    .Include(b => b.User)
                    .Include(b => b.Combo)
                    .Include(b => b.BookingDrinks)
                        .ThenInclude(bd => bd.Drink)
                    .Select(b => new BookingDto
                    {
                        Id = b.Id,
                        UserId = b.UserId,
                        RoomId = b.RoomId,
                        ComboId = b.ComboId,
                        BookingTime = b.BookingTime,
                        CheckIn = b.CheckIn,
                        CheckOut = b.CheckOut,
                        TotalPrice = b.TotalPrice,
                        Status = b.Status,
                        CreatedBy = b.CreatedBy,
                        CreatedAt = b.CreatedAt,
                        UserFullName = b.User.FullName, // Chỉ fields cần, tránh chu kỳ
                        RoomNumber = b.Room.RoomNumber,
                        ComboName = b.Combo != null ? b.Combo.Name : null,
                        BookingDrinks = b.BookingDrinks.Select(bd => new BookingDrinkDto
                        {
                            Id = bd.Id,
                            DrinkId = bd.DrinkId,
                            Quantity = bd.Quantity,
                            DrinkName = bd.Drink.Name
                        }).ToList()
                    })
                    .ToListAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting bookings: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ khi lấy danh sách booking.", error = ex.Message });
            }
        }

        // === GIẢI THÍCH: Phương thức POST để kiểm tra tính khả dụng của phòng ===
        // Lý do: Kiểm tra xem phòng có bị xung đột lịch đặt không trước khi tạo booking mới ===
        [HttpPost("check-availability")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckAvailability([FromBody] AvailabilityRequestDto request)
        {
            try
            {
                Console.WriteLine($"Check availability: RoomId={request.RoomId}, CheckIn={request.CheckIn}, CheckOut={request.CheckOut}");
                var room = await _context.Rooms.FindAsync(request.RoomId);
                if (room == null)
                {
                    return BadRequest(new { message = "RoomId không hợp lệ." });
                }
                var conflictingBookings = await _context.Bookings
                    .Where(b => b.RoomId == request.RoomId &&
                                b.Status != "Cancelled" &&
                                b.CheckOut > request.CheckIn &&
                                b.CheckIn < request.CheckOut)
                    .Select(b => new { b.Id, b.CheckIn, b.CheckOut })
                    .ToListAsync();
                if (conflictingBookings.Any())
                {
                    Console.WriteLine($"Conflicting bookings found: {string.Join(", ", conflictingBookings.Select(b => $"Id={b.Id}, CheckIn={b.CheckIn}, CheckOut={b.CheckOut}"))}");
                    return Ok(new { available = false, conflicts = conflictingBookings });
                }
                return Ok(new { available = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking availability: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ khi kiểm tra tính khả dụng.", error = ex.Message });
            }
        }

        // === GIẢI THÍCH: Phương thức POST để tạo booking mới ===
        // Lý do: Tạo đặt phòng với kiểm tra đầy đủ (room, combo, drinks, voucher) và tính toán tổng giá ===
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
        {
            try
            {
                Console.WriteLine($"Received BookingCreateDto: RoomId={dto.RoomId}, ComboId={dto.ComboId}, UserId={dto.UserId}, StartTime={dto.StartTime}, EndTime={dto.EndTime}, DrinkIds={string.Join(",", dto.DrinkIds ?? new List<int>())}, VoucherCode={dto.VoucherCode}");
                // Lấy UserId từ JWT
                var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "User ID không hợp lệ hoặc thiếu trong token." });
                }
                // Kiểm tra RoomId
                var room = await _context.Rooms.FindAsync(dto.RoomId);
                if (room == null)
                {
                    return BadRequest(new { message = "RoomId không hợp lệ." });
                }
                // Kiểm tra ComboId nếu có
                Combo? combo = null;
                if (dto.ComboId.HasValue)
                {
                    combo = await _context.Combos.FindAsync(dto.ComboId);
                    if (combo == null)
                    {
                        return BadRequest(new { message = "ComboId không hợp lệ." });
                    }
                }
                // Kiểm tra tính khả dụng
                var conflictingBookings = await _context.Bookings
                    .Where(b => b.RoomId == dto.RoomId &&
                                b.Status != "Cancelled" &&
                                b.CheckOut > dto.StartTime &&
                                b.CheckIn < dto.EndTime)
                    .Select(b => new { b.Id, b.CheckIn, b.CheckOut })
                    .ToListAsync();
                if (conflictingBookings.Any())
                {
                    Console.WriteLine($"Conflicting bookings found: {string.Join(", ", conflictingBookings.Select(b => $"Id={b.Id}, CheckIn={b.CheckIn}, CheckOut={b.CheckOut}"))}");
                    return BadRequest(new { message = "Phòng không khả dụng cho ngày đã chọn.", conflicts = conflictingBookings });
                }
                // Map DTO sang Booking
                var booking = _mapper.Map<Booking>(dto);
                booking.UserId = userId; // Gán từ JWT
                booking.BookingTime = DateTime.UtcNow;
                booking.CheckIn = dto.StartTime;
                booking.CheckOut = dto.EndTime;
                booking.Status = "Pending";
                booking.CreatedAt = DateTime.UtcNow;
                booking.CreatedBy = userId;
                // Tính TotalPrice
                var nights = (decimal)(dto.EndTime - dto.StartTime).TotalDays;
                if (nights <= 0)
                {
                    return BadRequest(new { message = "EndTime phải sau StartTime." });
                }
                booking.TotalPrice = (decimal)room.PricePerNight * nights;
                if (combo != null)
                {
                    booking.TotalPrice += (decimal)combo.Price;
                }
                // Xử lý BookingDrinks
                if (dto.DrinkIds != null && dto.DrinkIds.Any())
                {
                    var drinks = await _context.Drinks
                        .Where(d => dto.DrinkIds.Contains(d.Id))
                        .ToListAsync();
                    if (drinks.Count != dto.DrinkIds.Count)
                    {
                        return BadRequest(new { message = "Một hoặc nhiều DrinkId không hợp lệ." });
                    }
                    booking.BookingDrinks = dto.DrinkIds
                        .Select(id => new BookingDrink
                        {
                            DrinkId = id,
                            Quantity = 1,
                            Booking = booking
                        })
                        .ToList();
                    booking.TotalPrice += drinks.Sum(d => (decimal)d.Price * 1);
                }
                // === GIẢI THÍCH: Áp dụng voucher nếu có mã ===
                // Lý do: Giảm giá TotalPrice dựa trên phần trăm Discount từ voucher, chỉ nếu voucher hợp lệ ===
                if (!string.IsNullOrEmpty(dto.VoucherCode))
                {
                    var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == dto.VoucherCode && v.IsActive && v.ExpiryDate >= DateTime.UtcNow);
                    if (voucher != null)
                    {
                        booking.TotalPrice -= booking.TotalPrice * (decimal)(voucher.Discount / 100.0);
                    }
                    else
                    {
                        return BadRequest(new { message = "Mã voucher không hợp lệ hoặc đã hết hạn." });
                    }
                }

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return Ok(booking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating booking: {ex.Message}, StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ khi tạo booking.", error = ex.Message });
            }
        }

        // === GIẢI THÍCH: Phương thức POST để xác nhận booking ===
        // Lý do: Cập nhật trạng thái booking từ Pending sang Confirmed, thường dùng cho admin/staff ===
        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return NotFound(new { message = "Booking không tìm thấy." });
                }
                booking.Status = "Confirmed";
                await _context.SaveChangesAsync();
                return Ok(booking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error confirming booking: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ khi xác nhận booking.", error = ex.Message });
            }
        }
    }

    // === GIẢI THÍCH: DTO cho Booking (để tránh chu kỳ khi serialize) ===
    // Lý do: Chọn fields cần thiết, tránh include entity đầy đủ gây cycle ===
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public int? ComboId { get; set; }
        public DateTime BookingTime { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserFullName { get; set; }
        public string RoomNumber { get; set; }
        public string ComboName { get; set; }
        public List<BookingDrinkDto> BookingDrinks { get; set; }
    }

    public class BookingDrinkDto
    {
        public int Id { get; set; }
        public int DrinkId { get; set; }
        public int Quantity { get; set; }
        public string DrinkName { get; set; }
    }

    public class AvailabilityRequestDto
    {
        public int RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}