using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using HotelBooking.DTOs.Auth;
using HotelBooking.Modules.Entity_Models;
using HotelBooking.DTOs;
using HotelBooking.Modules.Auth.DTOs;

namespace HotelBooking.Modules.Auth.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDTO dto);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email đã được sử dụng.");

            // Find role by RoleId
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
            if (role == null) throw new Exception("Vai trò không hợp lệ.");

            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = hashedPassword,
                IsMember = false,
                CreatedAt = DateTime.UtcNow,
                RoleId = dto.RoleId
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Load the user with role to generate token
            var userWithRole = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            return await GenerateJwtTokenAsync(userWithRole!);
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) throw new Exception("Email hoặc mật khẩu không đúng.");
            var passwordMatch = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!passwordMatch) throw new Exception("Email hoặc mật khẩu không đúng.");

            return await GenerateJwtTokenAsync(user);
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            try
            {
                // Ensure role is loaded
                if (user.Role == null)
                {
                    user.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
                }
                var roleName = user.Role?.Name ?? "Customer";
                Console.WriteLine($"Generating token for user ${user.Email} with role ${roleName}");

                // Chỉ gán role "Admin" nếu RoleId = 1
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName)
                };
                if (user.RoleId == 1) // Admin
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                else if (user.RoleId == 3) // Staff
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Staff"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                }
                claims.Add(new Claim("userId", user.Id.ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

                Console.WriteLine("JWT claims: " + string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}")));
                var jwtSettings = _config.GetSection("JwtSettings");
                var secretKey = jwtSettings["Key"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new Exception("JWT Key not configured properly");
                }
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(24), // Extended to 24 hours for testing
                    Issuer = jwtSettings["Issuer"],
                    Audience = jwtSettings["Audience"],
                    SigningCredentials = creds
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                Console.WriteLine($"Generated JWT: {tokenString}");

                try
                {
                    var principal = tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);
                    var subClaim = principal.FindFirst("sub")?.Value;
                    Console.WriteLine($"Token validation successful. Sub claim: {subClaim}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Token validation failed: {ex.Message}");
                }
                return tokenString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating JWT: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new Exception($"Failed to generate JWT token: {ex.Message}");
            }
        }
    }
}