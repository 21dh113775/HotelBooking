using System;

namespace HotelBooking.DTOs
{
    public class VoucherCreateDto
    {
        public string Code { get; set; }
        public int Discount { get; set; } // Phần trăm giảm giá
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}