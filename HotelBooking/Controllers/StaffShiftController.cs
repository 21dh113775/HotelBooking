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
    public class StaffShiftController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StaffShiftController(AppDbContext context) => _context = context;

        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetShifts(int staffId)
        {
            var shifts = await _context.StaffShifts.Where(s => s.StaffId == staffId).ToListAsync();
            return Ok(shifts);
        }

        [HttpPost]
        public async Task<IActionResult> AssignShift(StaffShift shift)
        {
            _context.StaffShifts.Add(shift);
            await _context.SaveChangesAsync();
            return Ok(shift);
        }
    }

}
