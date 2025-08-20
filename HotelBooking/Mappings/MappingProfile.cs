using AutoMapper;
using HotelBooking.Domain.Entities;
using HotelBooking.DTOs;

namespace HotelBooking.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomCreateDto, Room>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Room, RoomCreateDto>();
        }
    }
}