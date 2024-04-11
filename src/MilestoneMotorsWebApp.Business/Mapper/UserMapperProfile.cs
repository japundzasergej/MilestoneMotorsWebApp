using AutoMapper;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, EditUserDto>()
                .ForMember(dest => dest.ProfilePictureImageUrl, opt => opt.Ignore());
        }
    }
}
