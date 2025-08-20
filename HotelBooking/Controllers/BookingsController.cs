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

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BookingController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

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
                    .ToListAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting bookings: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Internal server error while getting bookings.", error = ex.Message });
            }
        }

        [HttpPost("check-availability")]
        public async Task<IActionResult> CheckAvailability([FromBody] AvailabilityRequestDto request)
        {
            try
            {
                Console.WriteLine($"Check availability: RoomId={request.RoomId}, CheckIn={request.CheckIn}, CheckOut={request.CheckOut}");

                var room = await _context.Rooms.FindAsync(request.RoomId);
                if (room == null)
                {
                    return BadRequest(new { message = "Invalid RoomId." });
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
                return StatusCode(500, new { message = "Internal server error while checking availability.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
        {
            try
            {
                Console.WriteLine($"Received BookingCreateDto: RoomId={dto.RoomId}, ComboId={dto.ComboId}, UserId={dto.UserId}, StartTime={dto.StartTime}, EndTime={dto.EndTime}, DrinkIds={string.Join(",", dto.DrinkIds ?? new List<int>())}");

                // Validate RoomId
                var room = await _context.Rooms.FindAsync(dto.RoomId);
                if (room == null)
                {
                    return BadRequest(new { message = "Invalid RoomId." });
                }

                // Validate ComboId if provided
                Combo? combo = null;
                if (dto.ComboId.HasValue)
                {
                    combo = await _context.Combos.FindAsync(dto.ComboId);
                    if (combo == null)
                    {
                        return BadRequest(new { message = "Invalid ComboId." });
                    }
                }

                // Validate UserId
                var user = await _context.Users.FindAsync(dto.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid UserId." });
                }

                // Check availability
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
                    return BadRequest(new { message = "Room is not available for the selected dates.", conflicts = conflictingBookings });
                }

                // Map DTO to Booking
                var booking = _mapper.Map<Booking>(dto);
                booking.BookingTime = DateTime.UtcNow;
                booking.CheckIn = dto.StartTime;
                booking.CheckOut = dto.EndTime;
                booking.Status = "Pending";
                booking.CreatedAt = DateTime.UtcNow;
                booking.CreatedBy = dto.UserId ?? Convert.ToInt32(User.FindFirst("sub")?.Value); // Gán CreatedBy từ UserId hoặc JWT

                // Calculate TotalPrice
                var nights = (decimal)(dto.EndTime - dto.StartTime).TotalDays;
                if (nights <= 0)
                {
                    return BadRequest(new { message = "EndTime must be after StartTime." });
                }
                booking.TotalPrice = (decimal)room.PricePerNight * nights;
                if (combo != null)
                {
                    booking.TotalPrice += (decimal)combo.Price;
                }

                // Handle BookingDrinks
                if (dto.DrinkIds != null && dto.DrinkIds.Any())
                {
                    var drinks = await _context.Drinks
                        .Where(d => dto.DrinkIds.Contains(d.Id))
                        .ToListAsync();
                    if (drinks.Count != dto.DrinkIds.Count)
                    {
                        return BadRequest(new { message = "One or more DrinkIds are invalid." });
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
                return StatusCode(500, new { message = "Internal server error while creating booking.", error = ex.Message });
            }
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return NotFound(new { message = "Booking not found." });
                }
                booking.Status = "Confirmed";
                await _context.SaveChangesAsync();
                return Ok(booking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error confirming booking: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Internal server error while confirming booking.", error = ex.Message });
            }
        }
    }

    public class AvailabilityRequestDto
    {
        public int RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}