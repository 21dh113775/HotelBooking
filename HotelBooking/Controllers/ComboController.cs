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
    public class ComboController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ComboController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _context.Combos.Include(c => c.ComboDetails).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Combo combo)
        {
            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();
            return Ok(combo);
        }
    }

}
