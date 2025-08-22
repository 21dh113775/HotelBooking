using HotelBooking.DTOs.Auth;
using HotelBooking.Modules.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HotelBooking.DTOs;
using HotelBooking.Modules.Auth.DTOs;


namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;

        public AuthController(IAuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                Console.WriteLine($"Register request: {dto.Email}");
                var token = await _authService.RegisterAsync(dto);
                Console.WriteLine($"Register successful, token generated");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            try
            {
                Console.WriteLine($"Login request: {dto.Email}");
                var token = await _authService.LoginAsync(dto);
                Console.WriteLine($"Login successful, token generated");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                // Log all claims for debugging
                Console.WriteLine("=== JWT Claims Debug ===");
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                }
                Console.WriteLine("========================");

                // Try multiple ways to get user ID
                string? userIdStr = null;

                // Try 'sub' claim first (standard)
                userIdStr = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userIdStr))
                {
                    // Try ClaimTypes.NameIdentifier
                    userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                if (string.IsNullOrEmpty(userIdStr))
                {
                    // Try custom 'userId' claim
                    userIdStr = User.FindFirst("userId")?.Value;
                }

                Console.WriteLine($"Retrieved userIdStr: {userIdStr}");

                if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                {
                    Console.WriteLine($"Failed to parse user ID. userIdStr = '{userIdStr}'");
                    return Unauthorized(new { message = "Invalid or missing user ID in token." });
                }

                Console.WriteLine($"Parsed userId: {userId}");

                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    Console.WriteLine($"User not found with ID: {userId}");
                    return NotFound(new { message = "User not found." });
                }

                Console.WriteLine($"User found: {user.Email}, Role: {user.Role?.Name}");

                var response = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    phoneNumber = user.PhoneNumber,
                    role = user.Role?.Name ?? "Customer"
                };

                Console.WriteLine($"Profile response: {System.Text.Json.JsonSerializer.Serialize(response)}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting profile: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    message = "Internal server error while getting profile.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Chỉ admin mới truy cập được");
        }

        // Debug endpoint to check token
        [HttpGet("debug-token")]
        [Authorize]
        public IActionResult DebugToken()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new { claims });
        }
    }
}