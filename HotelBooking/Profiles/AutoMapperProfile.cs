using AutoMapper;
using HotelBooking.Domain.Entities;
using HotelBooking.DTOs.Booking;
using HotelBooking.DTOs.Combo;
using HotelBooking.DTOs.Drink;
using HotelBooking.DTOs.Furniture;
using HotelBooking.DTOs.Rom;
using HotelBooking.DTOs.Room;
using HotelBooking.Modules.Entity_Models;


namespace HotelBooking.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Room
            CreateMap<RoomCreateDto, Room>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            // Furniture
            CreateMap<FurnitureCreateDto, Furniture>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            // Combo
            CreateMap<ComboCreateDto, Combo>()
                .ForMember(dest => dest.ComboDetails, opt => opt.Ignore());

            // Drink
            CreateMap<Drink, DrinkDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
            // Booking
            CreateMap<BookingCreateDto, Booking>()
                .ForMember(dest => dest.BookingDrinks, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }

}
