using HotelBooking.Domain.Entities;
using HotelBooking.DTOs;
using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public RoomController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RoomCreateDto dto)
        {
            try
            {
                // Validate DTO
                if (string.IsNullOrEmpty(dto.RoomNumber))
                {
                    return BadRequest(new { message = "RoomNumber is required." });
                }

                // Check for duplicate RoomNumber
                if (await _context.Rooms.AnyAsync(r => r.RoomNumber == dto.RoomNumber))
                {
                    return BadRequest(new { message = $"Room number {dto.RoomNumber} already exists." });
                }

                var room = _mapper.Map<Room>(dto);
                Console.WriteLine($"Mapped Room: {room.RoomNumber}, {room.PricePerNight}, {room.IsAvailable}, {room.Description}, {room.ImageUrl}");

                if (dto.Image != null)
                {
                    var uploadsDir = Path.Combine(_env.WebRootPath ?? Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

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
                Console.WriteLine($"Error saving room: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return BadRequest(new { message = ex.Message, innerException = ex.InnerException?.Message });
            }
        }

        [HttpOptions]
        public IActionResult Options()
        {
            return Ok();
        }
    }
}