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
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BookingController(AppDbContext context) => _context = context;

        [HttpPost("check-availability")]
        public async Task<IActionResult> CheckAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var isAvailable = !await _context.Bookings
                .AnyAsync(b => b.RoomId == roomId &&
                               b.CheckOut > checkIn &&
                               b.CheckIn < checkOut &&
                               b.Status != "Cancelled");

            return Ok(new { available = isAvailable });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            booking.BookingTime = DateTime.UtcNow;
            booking.Status = "Pending";
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return Ok(booking);
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            booking.Status = "Confirmed";
            await _context.SaveChangesAsync();
            return Ok(booking);
        }
    }

}
