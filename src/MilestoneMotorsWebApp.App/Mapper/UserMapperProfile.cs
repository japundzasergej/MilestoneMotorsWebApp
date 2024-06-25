using AutoMapper;
using MilestoneMotorsWebApp.App.Helpers;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<RegisterUserViewModel, RegisterUserDto>().ReverseMap();
            CreateMap<LoginUserViewModel, LoginUserDto>().ReverseMap();
            CreateMap<EditUserDto, EditUserViewModel>();
            CreateMap<UserDto, UserAccountViewModel>();
            CreateMap<EditUserViewModel, EditUserDto>()
                .ForMember(
                    dest => dest.ProfilePictureImageUrl,
                    opt =>
                        opt.MapFrom(
                            src =>
                                PhotoHelpers.ConvertFormFileToByteArray(src.ProfilePictureImageUrl)
                        )
                );
        }
    }
}
