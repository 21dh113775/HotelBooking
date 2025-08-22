using AutoMapper;
using HotelBooking.Domain.Entities;
using HotelBooking.DTOs;
using HotelBooking.DTOs.Booking;
using HotelBooking.Modules.Auth.DTOs;
using HotelBooking.Modules.Entity_Models;

namespace HotelBooking.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomCreateDto, Room>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Room, RoomCreateDto>();

            CreateMap<BookingCreateDto, Booking>()
                .ForMember(dest => dest.CheckIn, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.CheckOut, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.BookingDrinks, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.BookingTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}