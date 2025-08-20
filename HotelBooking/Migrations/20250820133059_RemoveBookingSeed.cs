using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookingSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookingDrinks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookingDrinks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$9mbkuR2C/saRpIXf3a5Xr.WqH8oVYed.WAfLgAmkmpA4OBiDqDzWW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingTime", "CheckIn", "CheckOut", "ComboId", "CreatedAt", "CreatedBy", "RoomId", "Status", "TotalPrice", "UserId" },
                values: new object[] { 1, new DateTime(2025, 8, 20, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 21, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 23, 12, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 8, 20, 11, 0, 0, 0, DateTimeKind.Utc), 1, 1, "Confirmed", 2110000m, 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$A7FwYd/9mZ0XqroXDT30teHG4VzVViKtx67OM.IT62ZKLzBBUEEXe");

            migrationBuilder.InsertData(
                table: "BookingDrinks",
                columns: new[] { "Id", "BookingId", "DrinkId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 2 },
                    { 2, 1, 2, 1 }
                });
        }
    }
}
