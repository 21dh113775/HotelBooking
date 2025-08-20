using AutoMapper;
using HotelBooking.Domain.Entities;
using HotelBooking.DTOs.Rom;
using HotelBooking.DTOs.Rom;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Modules.Entity_Models;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] RoomCreateDto dto)
    {
        var room = _mapper.Map<Room>(dto);

        if (dto.Image != null)
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var path = Path.Combine(_env.WebRootPath, "uploads", imageName);
            using var stream = new FileStream(path, FileMode.Create);
            await dto.Image.CopyToAsync(stream);
            room.ImageUrl = "/uploads/" + imageName;
        }

        room.CreatedAt = DateTime.UtcNow;
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return Ok(room);
    }
}
