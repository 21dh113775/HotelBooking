🏨 Hệ Thống Backend Đặt Phòng Khách Sạn

Dự án cá nhân: API Backend cho ứng dụng đặt phòng khách sạn
Mã số sinh viên: 21DH113775
Trường: Đại học Ngoại Ngữ Tin Học TP.HCM  


🎯 Tổng Quan
HotelBooking là hệ thống backend API RESTful được xây dựng bằng ASP.NET Core 8.0, hỗ trợ quản lý đặt phòng khách sạn, người dùng, phòng, nội thất, voucher, và ca làm việc nhân viên. Dự án sử dụng JWT để xác thực, Entity Framework Core để quản lý cơ sở dữ liệu SQL Server, và FluentValidation để kiểm tra dữ liệu.

⚡ Tính Năng Chính

Quản Lý Người Dùng: Đăng ký, đăng nhập, phân quyền (Admin, Manager, Staff, Customer).
Quản Lý Phòng: CRUD phòng, gán nội thất (TV, bàn làm việc).
Đặt Phòng: Tạo, cập nhật, hủy đặt phòng, tích hợp đồ uống và combo.
Voucher: Quản lý mã giảm giá với kiểm tra hợp lệ.
Ca Làm Việc: Theo dõi ca làm việc của nhân viên.
Bảo Mật: JWT, BCrypt cho mã hóa mật khẩu.


🛠️ Công Nghệ

Backend: ASP.NET Core 8.0, Entity Framework Core
Cơ Sở Dữ Liệu: SQL Server 2019+
Bảo Mật: JWT, BCrypt, FluentValidation
Công Cụ: AutoMapper, Swagger/OpenAPI
Kiến Trúc: Clean Architecture, Repository Pattern


📋 Yêu Cầu Hệ Thống

.NET SDK: 8.0+
SQL Server: 2019+ (hoặc LocalDB)
Công cụ: Visual Studio 2022, Git, Postman
RAM: 8GB+ (khuyến nghị 16GB)


🚀 Hướng Dẫn Cài Đặt

Clone Repository:
git clone https://github.com/21dh113775/HotelBooking.git
cd HotelBooking


Cài Đặt Thư Viện:
dotnet restore


Cấu Hình Cơ Sở Dữ Liệu:Tạo tệp appsettings.json:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=HotelBookingDB;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "Key": "YourSecretKeyMustBeAtLeast32CharactersLong!@#$%",
    "Issuer": "HotelBookingAPI",
    "Audience": "HotelBookingClient"
  }
}


Chạy Migration:
dotnet ef migrations add InitialCreate
dotnet ef database update


Khởi Động Ứng Dụng:
dotnet run

Truy cập Swagger tại: https://localhost:<port>/swagger



📖 API Chính



Phương Thức
Đường Dẫn
Mô Tả
Xác Thực



POST
/api/auth/register
Đăng ký tài khoản mới
Không


POST
/api/auth/login
Đăng nhập, nhận JWT token
Không


GET
/api/auth/profile
Xem hồ sơ người dùng
JWT


GET
/api/auth/admin-only
API chỉ dành cho Admin
JWT (Admin)


Ví Dụ Gọi API

Đăng ký:
curl -X POST https://localhost:<port>/api/auth/register \
-H "Content-Type: application/json" \
-d '{"fullName": "Nguyen Van A", "email": "user@example.com", "phoneNumber": "0123456789", "password": "Password@123", "role": "Customer"}'


Đăng nhập:
curl -X POST https://localhost:<port>/api/auth/login \
-H "Content-Type: application/json" \
-d '{"email": "user@example.com", "password": "Password@123"}'




🗄️ Cấu Trúc Cơ Sở Dữ Liệu
Bảng Chính

Users: Lưu thông tin người dùng (Email, PasswordHash, RoleId).
Rooms: Lưu thông tin phòng (RoomNumber, PricePerNight, IsAvailable).
Bookings: Lưu thông tin đặt phòng (UserId, RoomId, CheckInDate, TotalAmount).
Vouchers: Lưu mã giảm giá (Code, Discount, ExpiryDate).
RoomFurniture: Bảng trung gian cho quan hệ nhiều-nhiều giữa phòng và nội thất.

Dữ Liệu Mẫu

Tài khoản Admin: admin@hotel.com / Admin@123
Vai trò: Admin, Manager, Staff, Customer
Nội thất: TV Samsung, Bàn làm việc
Voucher: Mã GIAM10 (giảm 10%)


🔧 Hướng Dẫn Phát Triển

Thêm Migration:
dotnet ef migrations add <TênMigration>
dotnet ef database update


Kiểm tra API: Sử dụng Postman hoặc Swagger UI.



🚀 Triển Khai

Chuẩn bị:

Sử dụng Azure, AWS, hoặc VPS (FPT Cloud, Viettel Cloud).
Cập nhật appsettings.Production.json với chuỗi kết nối SQL Server.


Triển khai với Docker:Tạo Dockerfile:
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY bin/Release/net8.0/publish/ app/
WORKDIR /app
ENTRYPOINT ["dotnet", "HotelBooking.dll"]

Build và chạy:
dotnet publish -c Release
docker build -t hotel-booking-backend .
docker run -p 8080:80 hotel-booking-backend




🤝 Đóng Góp

Fork repository.
Tạo nhánh mới: git checkout -b feature/tinh-nang-moi.
Commit thay đổi: git commit -m "Thêm tính năng mới".
Push lên nhánh: git push origin feature/tinh-nang-moi.
Mở Pull Request trên GitHub.


📜 Giấy Phép
Dự án được cấp phép theo MIT License.

👨‍🎓 Thông Tin Tác Giả

Tên: Lê Trần Đăng Khoa
Email: lekhoale30092003@gmail.com
GitHub: 21dh113775
