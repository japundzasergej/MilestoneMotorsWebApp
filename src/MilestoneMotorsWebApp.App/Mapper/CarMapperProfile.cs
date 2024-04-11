using AutoMapper;
using MilestoneMotorsWebApp.App.Helpers;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Utilities;

namespace MilestoneMotorsWebApp.App.Mapper
{
    public class CarMapperProfile : Profile
    {
        public CarMapperProfile()
        {
            CreateMap<CarDto, GetCarViewModel>()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => ConvertToEuroMethod.ConvertToEuro(src.Price))
                )
                .ForMember(
                    dest => dest.Model,
                    opt => opt.MapFrom(src => src.Model.FirstCharToUpper().Trim())
                )
                .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => $"{src.Mileage} (km)"))
                .ForMember(
                    dest => dest.EngineCapacity,
                    opt => opt.MapFrom(src => $"{src.EngineCapacity} (cm3)")
                )
                .ForMember(
                    dest => dest.EnginePower,
                    opt => opt.MapFrom(src => $"{src.EnginePower} (kW/hP)")
                );
            CreateMap<CreateCarViewModel, CreateCarDto>()
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description.Trim().FirstCharToUpper())
                )
                .ForMember(
                    dest => dest.AdNumber,
                    opt => opt.MapFrom(src => $"{src.UserId}-{src.Model}")
                )
                .ForMember(
                    dest => dest.HeadlinerImageUrl,
                    opt =>
                        opt.MapFrom(
                            src => PhotoHelpers.ConvertFormFileToByteArray(src.HeadlinerImageUrl)
                        )
                )
                .ForMember(
                    dest => dest.PhotoOne,
                    opt => opt.MapFrom(src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoOne))
                )
                .ForMember(
                    dest => dest.PhotoTwo,
                    opt => opt.MapFrom(src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoTwo))
                )
                .ForMember(
                    dest => dest.PhotoThree,
                    opt =>
                        opt.MapFrom(src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoThree))
                )
                .ForMember(
                    dest => dest.PhotoFour,
                    opt =>
                        opt.MapFrom(src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoFour))
                )
                .ForMember(
                    dest => dest.PhotoFive,
                    opt =>
                        opt.MapFrom(src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoFive))
                );

            CreateMap<EditCarDto, EditCarViewModel>();

            CreateMap<EditCarViewModel, EditCarDto>();
        }
    }
}
