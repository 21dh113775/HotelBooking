using HotelBooking.Domain.Entities;
using HotelBooking.Modules.Entity_Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<RoomFurniture> RoomFurnitures { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDrink> BookingDrinks { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<StaffShift> StaffShifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Many-to-Many: Room - Furniture ===
            modelBuilder.Entity<RoomFurniture>()
                .HasKey(rf => new { rf.RoomId, rf.FurnitureId });

            // === One-to-Many: ComboDetail ===
            modelBuilder.Entity<ComboDetail>()
                .HasKey(cd => cd.Id);

            // === BookingDrink ===
            modelBuilder.Entity<BookingDrink>()
                .HasKey(bd => bd.Id);

            // === StaffShift ===
            modelBuilder.Entity<StaffShift>()
                .HasKey(ss => ss.Id);

            // === Seeding Roles ===
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Manager" },
                new Role { Id = 3, Name = "Staff" },
                new Role { Id = 4, Name = "Customer" }
            );

            // === Seeding Admin ===
            var adminPass = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Admin",
                    Email = "admin@hotel.com",
                    PhoneNumber = "0123456789",
                    PasswordHash = adminPass,
                    RoleId = 1,
                    IsMember = false,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // === Seed Furnitures ===
            modelBuilder.Entity<Furniture>().HasData(
                new Furniture { Id = 1, Name = "Tivi Samsung", Description = "Smart TV 43 inch", Price = 25000, ImageUrl = "/images/furniture/tv.jpg" },
                new Furniture { Id = 2, Name = "Bàn làm việc", Description = "Bàn gỗ cao cấp", Price = 15000, ImageUrl = "/images/furniture/desk.jpg" }
            );

            // === Seed RoomFurniture mapping ===
            modelBuilder.Entity<RoomFurniture>().HasData(
                new RoomFurniture { RoomId = 1, FurnitureId = 1 },
                new RoomFurniture { RoomId = 1, FurnitureId = 2 },
                new RoomFurniture { RoomId = 2, FurnitureId = 1 }
            );

            // === Seed Drinks ===
            modelBuilder.Entity<Drink>().HasData(
                new Drink { Id = 1, Name = "Nước suối", Price = 10000, ImageUrl = "/images/drinks/water.jpg" },
                new Drink { Id = 2, Name = "Cà phê lon", Price = 15000,  ImageUrl = "/images/drinks/coffee.jpg" }
            );

            // === Seed Voucher ===
            modelBuilder.Entity<Voucher>().HasData(
                new Voucher { Id = 1, Code = "GIAM10", Discount = 10, ExpiryDate = DateTime.UtcNow.AddMonths(3), IsActive = true }
            );

            // === Seed Combo & ComboDetail ===
            modelBuilder.Entity<Combo>().HasData(
                new Combo { Id = 1, Name = "Combo Phòng + Nội Thất", Description = "Phòng Deluxe kèm TV", ComboType = "RoomFurniture", Price = 110000 }
            );

            modelBuilder.Entity<ComboDetail>().HasData(
                new ComboDetail { Id = 1, ComboId = 1, RoomId = 1, FurnitureId = 1 }
            );

            // NOTE: Nếu cần seed thêm Room, Booking,... thì cần chắc chắn các Id liên quan đã tồn tại để tránh lỗi.
        }
    }
}
