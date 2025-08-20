# 🏨 Hệ Thống Backend Đặt Phòng Khách Sạn

> **Dự án tốt nghiệp**: Hệ thống API Backend cho ứng dụng đặt phòng khách sạn trực tuyến  
> **Công nghệ**: ASP.NET Core 8.0 | Entity Framework Core | SQL Server

[![.NET Version](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C# Version](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## 📚 Mục Lục

- [🎯 Tổng Quan Dự Án](#-tổng-quan-dự-án)
- [⚡ Tính Năng Nổi Bật](#-tính-năng-nổi-bật)
- [🛠️ Công Nghệ & Kiến Trúc](#%EF%B8%8F-công-nghệ--kiến-trúc)
- [📋 Yêu Cầu Hệ Thống](#-yêu-cầu-hệ-thống)
- [🚀 Hướng Dẫn Cài Đặt](#-hướng-dẫn-cài-đặt)
- [⚙️ Cấu Hình Môi Trường](#%EF%B8%8F-cấu-hình-môi-trường)
- [📖 API Documentation](#-api-documentation)
- [🗄️ Cấu Trúc Database](#%EF%B8%8F-cấu-trúc-database)
- [🔧 Hướng Dẫn Phát Triển](#-hướng-dẫn-phát-triển)
- [🧪 Testing & Quality Assurance](#-testing--quality-assurance)
- [🚀 Triển Khai Production](#-triển-khai-production)
- [📈 Roadmap & Tương Lai](#-roadmap--tương-lai)
- [🤝 Đóng Góp & Cộng Tác](#-đóng-góp--cộng-tác)
- [👨‍🎓 Thông Tin Tác Giả](#-thông-tin-tác-giả)

---

## 🎯 Tổng Quan Dự Án

**HotelBooking Backend** là hệ thống API RESTful được phát triển với **ASP.NET Core 8.0**, cung cấp đầy đủ các chức năng backend cho ứng dụng đặt phòng khách sạn chuyên nghiệp. Dự án được thiết kế với kiến trúc sạch (Clean Architecture), đảm bảo tính mở rộng, bảo trì và hiệu suất cao.

### 🎓 Thông Tin Học Tập
- **Sinh viên**: [Họ và tên của bạn]
- **Mã số sinh viên**: **21DH113775**
- **Trường**: Đại học Công nghiệp Thành phố Hồ Chí Minh
- **Khoa**: Công nghệ Thông tin
- **Môn học**: Đồ án tốt nghiệp / Thực tập doanh nghiệp
- **Giảng viên hướng dẫn**: [Tên giảng viên]
- **Năm học**: 2024 - 2025

### 🌟 Điểm Nổi Bật
- ✅ **Kiến trúc hiện đại**: Clean Architecture với Dependency Injection
- ✅ **Bảo mật cao**: JWT Authentication với role-based authorization
- ✅ **Hiệu suất tối ưu**: Entity Framework Core với Query Optimization
- ✅ **Validation mạnh mẽ**: FluentValidation cho tất cả input
- ✅ **Documentation tự động**: Swagger/OpenAPI 3.0
- ✅ **Code quality**: Tuân thủ SOLID principles và Design patterns

---

## ⚡ Tính Năng Nổi Bật

### 🔐 **Hệ Thống Quản Lý Người Dùng**
- **Xác thực đa cấp**: Đăng ký, đăng nhập với JWT token
- **Phân quyền chi tiết**: 4 vai trò (Admin, Manager, Staff, Customer)
- **Quản lý profile**: Cập nhật thông tin cá nhân, đổi mật khẩu
- **Bảo mật nâng cao**: BCrypt password hashing, token refresh

### 🏨 **Quản Lý Khách Sạn & Phòng**
- **CRUD hoàn chỉnh**: Tạo, đọc, cập nhật, xóa phòng
- **Quản lý nội thất**: Gán và quản lý thiết bị phòng (TV, bàn làm việc, v.v.)
- **Hệ thống combo**: Gói combo phòng + nội thất tùy chỉnh
- **Trạng thái phòng**: Theo dõi tình trạng phòng real-time

### 📅 **Hệ Thống Đặt Phòng Thông Minh**
- **Đặt phòng linh hoạt**: Hỗ trợ đặt nhiều phòng, nhiều ngày
- **Quản lý trạng thái**: Pending, Confirmed, Cancelled, Completed
- **Tích hợp dịch vụ**: Thêm đồ uống, combo vào booking
- **Lịch sử chi tiết**: Theo dõi đầy đủ lịch sử đặt phòng

### 🎫 **Hệ Thống Voucher & Khuyến Mãi**
- **Mã giảm giá linh hoạt**: Phần trăm hoặc số tiền cố định
- **Điều kiện áp dụng**: Giới hạn thời gian, số lượng sử dụng
- **Validation tự động**: Kiểm tra tính hợp lệ của voucher

### 👥 **Quản Lý Nhân Sự**
- **Ca làm việc**: Phân ca và theo dõi giờ làm việc nhân viên
- **Phân quyền công việc**: Gán nhiệm vụ theo vai trò
- **Báo cáo hiệu suất**: Thống kê giờ làm và năng suất

### 🍹 **Quản Lý Dịch Vụ Bổ Sung**
- **Menu đồ uống**: Quản lý thực đơn room service
- **Tích hợp booking**: Tự động thêm vào hóa đơn
- **Quản lý giá**: Linh hoạt cập nhật giá theo thời gian

---

## 🛠️ Công Nghệ & Kiến Trúc

### 🏗️ **Backend Framework**
```csharp
ASP.NET Core 8.0          // Web API Framework
Entity Framework Core     // ORM và Database Access
SQL Server 2019+         // Relational Database Management

### 🔒 **Bảo Mật & Xác Thực**
```csharp
JWT (JSON Web Token)      // Stateless Authentication
BCrypt.Net               // Password Hashing
FluentValidation         // Input Validation
CORS Policy              // Cross-Origin Resource Sharing
### 📊 **Quản Lý Dữ Liệu**
```csharp
AutoMapper               // Object-to-Object Mapping
Repository Pattern       // Data Access Abstraction
Unit of Work Pattern     // Transaction Management
🏛️ Kiến Trúc Hệ Thống
┌─────────────────────────────────────────────┐
│                Controllers                  │ ← Presentation Layer
├─────────────────────────────────────────────┤
│              Services/BLL                   │ ← Business Logic Layer
├─────────────────────────────────────────────┤
│            Repositories/DAL                 │ ← Data Access Layer
├─────────────────────────────────────────────┤
│         Entity Framework Core               │ ← ORM Layer
├─────────────────────────────────────────────┤
│              SQL Server                     │ ← Database Layer
└─────────────────────────────────────────────┘

📋 Yêu Cầu Hệ Thống
💻 Môi Trường Phát Triển
Thành phầnPhiên bản tối thiểuKhuyến nghị.NET SDK8.08.0.x (Latest)SQL Server20192022 DeveloperVisual Studio20222022 Community/ProIIS Express10.0LatestRAM8GB16GB+Storage5GBSSD 10GB+
🛠️ Công Cụ Hỗ Trợ

Git - Version control system
Postman - API testing và documentation
SQL Server Management Studio (SSMS) - Database management
Visual Studio Code - Alternative lightweight editor
Docker Desktop - Containerization (optional)

--------------
🚀 Hướng Dẫn Cài Đặt
1️⃣ Clone Repository
bash# Clone project từ GitHub
git clone https://github.com/21dh113775/HotelBooking.git

# Di chuyển vào thư mục project
cd HotelBooking
2️⃣ Cài Đặt Dependencies
bash# Restore NuGet packages
dotnet restore

# Verify cài đặt
dotnet --version
3️⃣ Cấu Hình Database
bash# Cài đặt EF Core Tools (nếu chưa có)
dotnet tool install --global dotnet-ef

# Verify EF Tools
dotnet ef --version
4️⃣ Tạo Database
bash# Tạo migration đầu tiên
dotnet ef migrations add InitialCreate

# Cập nhật database
dotnet ef database update
5️⃣ Khởi Chạy Ứng Dụng
bash# Chạy trong Development mode
dotnet run

# Hoặc sử dụng Visual Studio (F5)
6️⃣ Kiểm Tra Kết Quả

API Swagger: https://localhost:7001/swagger
Health Check: https://localhost:7001/health
Base API: https://localhost:7001/api/


⚙️ Cấu Hình Môi Trường
📄 appsettings.json
json{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=HotelBookingDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "Key": "YourSecretKeyMustBeAtLeast32CharactersLong!@#$%",
    "Issuer": "HotelBookingAPI",
    "Audience": "HotelBookingClient",
    "ExpiryInMinutes": 60,
    "RefreshTokenExpiryInDays": 7
  },
  "AppSettings": {
    "ApiVersion": "v1",
    "AllowedOrigins": "http://localhost:3000,http://localhost:3001",
    "MaxUploadSize": 5242880,
    "DefaultPageSize": 20
  }
}
🔒 appsettings.Production.json
json{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "[Production SQL Server Connection String]"
  },
  "JwtSettings": {
    "Key": "[Production JWT Secret - Minimum 32 characters]",
    "ExpiryInMinutes": 30
  }
}
🌍 Environment Variables
bash# Development
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:7001;http://localhost:5001

# Production
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://+:80;https://+:443

📖 API Documentation
🔗 Base URL
Development: https://localhost:7001/api
Production:  https://your-domain.com/api
🔐 Authentication Endpoints
MethodEndpointMô tảAuth RequiredPOST/auth/registerĐăng ký tài khoản mới❌POST/auth/loginĐăng nhập và nhận JWT token❌GET/auth/profileLấy thông tin profile người dùng✅PUT/auth/profileCập nhật thông tin profile✅POST/auth/change-passwordĐổi mật khẩu✅POST/auth/refresh-tokenLàm mới JWT token✅GET/auth/admin-onlyEndpoint test cho Admin✅ (Admin)
🏨 Room Management Endpoints
MethodEndpointMô tảAuth RequiredGET/roomsLấy danh sách phòng (có phân trang)❌GET/rooms/{id}Lấy chi tiết phòng theo ID❌POST/roomsTạo phòng mới✅ (Manager+)PUT/rooms/{id}Cập nhật thông tin phòng✅ (Manager+)DELETE/rooms/{id}Xóa phòng✅ (Admin)GET/rooms/availableLấy danh sách phòng trống❌
📅 Booking Management Endpoints
MethodEndpointMô tảAuth RequiredGET/bookingsLấy danh sách booking✅GET/bookings/{id}Lấy chi tiết booking✅POST/bookingsTạo booking mới✅ (Customer+)PUT/bookings/{id}Cập nhật booking✅DELETE/bookings/{id}Hủy booking✅POST/bookings/{id}/confirmXác nhận booking✅ (Staff+)
🎫 Voucher Management Endpoints
MethodEndpointMô tảAuth RequiredGET/vouchersLấy danh sách voucher✅ (Staff+)GET/vouchers/{code}Kiểm tra voucher theo mã✅POST/vouchersTạo voucher mới✅ (Manager+)PUT/vouchers/{id}Cập nhật voucher✅ (Manager+)DELETE/vouchers/{id}Xóa voucher✅ (Admin)POST/vouchers/validateValidate mã voucher✅
📊 Dashboard & Reports Endpoints
MethodEndpointMô tảAuth RequiredGET/dashboard/statsThống kê tổng quan✅ (Staff+)GET/dashboard/revenueBáo cáo doanh thu✅ (Manager+)GET/dashboard/bookings/monthlyThống kê booking theo tháng✅ (Staff+)GET/dashboard/rooms/occupancyTỷ lệ lấp đầy phòng✅ (Manager+)
📋 Request/Response Examples
Đăng Ký Tài Khoản
bashPOST /api/auth/register
Content-Type: application/json

{
  "fullName": "Nguyễn Văn An",
  "email": "nguyenvanan@email.com",
  "phoneNumber": "0123456789",
  "password": "Password@123",
  "role": "Customer"
}
Response:
json{
  "success": true,
  "message": "Đăng ký tài khoản thành công",
  "data": {
    "id": "12345678-1234-1234-1234-123456789012",
    "fullName": "Nguyễn Văn An",
    "email": "nguyenvanan@email.com",
    "role": "Customer",
    "createdAt": "2024-01-15T10:30:00Z"
  }
}
Đăng Nhập
bashPOST /api/auth/login
Content-Type: application/json

{
  "email": "nguyenvanan@email.com",
  "password": "Password@123"
}
Response:
json{
  "success": true,
  "message": "Đăng nhập thành công",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiry": "2024-01-15T11:30:00Z",
    "user": {
      "id": "12345678-1234-1234-1234-123456789012",
      "fullName": "Nguyễn Văn An",
      "email": "nguyenvanan@email.com",
      "role": "Customer"
    }
  }
}
Tạo Booking Mới
bashPOST /api/bookings
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "roomId": "room-001",
  "checkInDate": "2024-02-01T14:00:00Z",
  "checkOutDate": "2024-02-03T12:00:00Z",
  "numberOfGuests": 2,
  "specialRequests": "Tầng cao, view biển",
  "voucherCode": "GIAM10",
  "beverages": [
    {
      "beverageId": "bev-001",
      "quantity": 2
    }
  ]
}
Response:
json{
  "success": true,
  "message": "Đặt phòng thành công",
  "data": {
    "bookingId": "booking-001",
    "bookingCode": "BK20240115001",
    "room": {
      "id": "room-001",
      "name": "Phòng Deluxe Ocean View",
      "pricePerNight": 1500000
    },
    "checkIn": "2024-02-01T14:00:00Z",
    "checkOut": "2024-02-03T12:00:00Z",
    "totalNights": 2,
    "subtotal": 3000000,
    "discount": 300000,
    "totalAmount": 2700000,
    "status": "Pending",
    "createdAt": "2024-01-15T10:30:00Z"
  }
}

🗄️ Cấu Trúc Database
📊 Entity Relationship Diagram
    User                    Booking                    Room
┌──────────┐         ┌──────────────┐         ┌──────────────┐
│ UserId   │◄─────1:n──┤ UserId (FK)  │         │ RoomId       │
│ Email    │         │ BookingId    │n:1─────►│ RoomNumber   │
│ Password │         │ RoomId (FK)  │         │ RoomType     │
│ FullName │         │ CheckIn      │         │ Price        │
│ Role     │         │ CheckOut     │         │ IsAvailable  │
└──────────┘         │ TotalAmount  │         └──────────────┘
                     │ Status       │
                     └──────────────┘
                            │
                         1:n│
                            ▼
                   ┌──────────────┐
                   │ BookingBeverage
                   ├──────────────┤
                   │ BookingId(FK)│
                   │ BeverageId(FK)│
                   │ Quantity     │
                   └──────────────┘
🏗️ Database Tables
👤 Users Table
sqlCREATE TABLE Users (
    UserId          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FullName        NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber     NVARCHAR(20),
    PasswordHash    NVARCHAR(MAX) NOT NULL,
    Role            NVARCHAR(50) NOT NULL DEFAULT 'Customer',
    IsActive        BIT DEFAULT 1,
    CreatedAt       DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2 DEFAULT GETUTCDATE()
);

-- Index for performance
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Role ON Users(Role);
🏨 Rooms Table
sqlCREATE TABLE Rooms (
    RoomId          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RoomNumber      NVARCHAR(10) NOT NULL UNIQUE,
    RoomType        NVARCHAR(50) NOT NULL,
    Description     NVARCHAR(MAX),
    PricePerNight   DECIMAL(10,2) NOT NULL,
    MaxGuests       INT DEFAULT 2,
    IsAvailable     BIT DEFAULT 1,
    CreatedAt       DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2 DEFAULT GETUTCDATE()
);

-- Index for queries
CREATE INDEX IX_Rooms_RoomType ON Rooms(RoomType);
CREATE INDEX IX_Rooms_IsAvailable ON Rooms(IsAvailable);
CREATE INDEX IX_Rooms_PricePerNight ON Rooms(PricePerNight);
📅 Bookings Table
sqlCREATE TABLE Bookings (
    BookingId       UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    BookingCode     NVARCHAR(20) NOT NULL UNIQUE,
    UserId          UNIQUEIDENTIFIER NOT NULL,
    RoomId          UNIQUEIDENTIFIER NOT NULL,
    CheckInDate     DATETIME2 NOT NULL,
    CheckOutDate    DATETIME2 NOT NULL,
    NumberOfGuests  INT DEFAULT 1,
    Subtotal        DECIMAL(10,2) NOT NULL,
    DiscountAmount  DECIMAL(10,2) DEFAULT 0,
    TotalAmount     DECIMAL(10,2) NOT NULL,
    Status          NVARCHAR(20) DEFAULT 'Pending',
    SpecialRequests NVARCHAR(MAX),
    CreatedAt       DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2 DEFAULT GETUTCDATE(),
    
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);

-- Indexes for performance
CREATE INDEX IX_Bookings_UserId ON Bookings(UserId);
CREATE INDEX IX_Bookings_RoomId ON Bookings(RoomId);
CREATE INDEX IX_Bookings_CheckInDate ON Bookings(CheckInDate);
CREATE INDEX IX_Bookings_Status ON Bookings(Status);
🎫 Vouchers Table
sqlCREATE TABLE Vouchers (
    VoucherId       UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Code            NVARCHAR(20) NOT NULL UNIQUE,
    Name            NVARCHAR(100) NOT NULL,
    Description     NVARCHAR(MAX),
    DiscountType    NVARCHAR(20) DEFAULT 'Percentage', -- Percentage, FixedAmount
    DiscountValue   DECIMAL(10,2) NOT NULL,
    MinAmount       DECIMAL(10,2) DEFAULT 0,
    MaxDiscount     DECIMAL(10,2),
    UsageLimit      INT,
    UsedCount       INT DEFAULT 0,
    StartDate       DATETIME2 NOT NULL,
    EndDate         DATETIME2 NOT NULL,
    IsActive        BIT DEFAULT 1,
    CreatedAt       DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2 DEFAULT GETUTCDATE()
);

CREATE INDEX IX_Vouchers_Code ON Vouchers(Code);
CREATE INDEX IX_Vouchers_IsActive ON Vouchers(IsActive);
🔄 Database Migrations
Tạo Migration Mới
bash# Thêm migration cho tính năng mới
dotnet ef migrations add AddVoucherTable

# Xem preview SQL commands
dotnet ef migrations script

# Apply migration
dotnet ef database update
Rollback Migration
bash# Rollback về migration cụ thể
dotnet ef database update PreviousMigrationName

# Remove migration chưa apply
dotnet ef migrations remove
