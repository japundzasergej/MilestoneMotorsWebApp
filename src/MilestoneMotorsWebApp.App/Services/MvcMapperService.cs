using AutoMapper;
using MilestoneMotorsWebApp.App.Helpers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Utilities;

namespace MilestoneMotorsWebApp.App.Services
{
    public class MvcMapperService : IMvcMapperService
    {
        private readonly IMapper _mapper;

        public MvcMapperService()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateCarViewModel, CreateCarDto>()
                    .ForMember(
                        dest => dest.Price,
                        opt => opt.MapFrom(src => ConvertToEuroMethod.ConvertToEuro(src.Price))
                    )
                    .ForMember(
                        dest => dest.Description,
                        opt => opt.MapFrom(src => src.Description.Trim().FirstCharToUpper())
                    )
                    .ForMember(
                        dest => dest.Model,
                        opt => opt.MapFrom(src => src.Model.FirstCharToUpper().Trim())
                    )
                    .ForMember(
                        dest => dest.Mileage,
                        opt => opt.MapFrom(src => $"{src.Mileage} (km)")
                    )
                    .ForMember(
                        dest => dest.EngineCapacity,
                        opt => opt.MapFrom(src => $"{src.EngineCapacity} (cm3)")
                    )
                    .ForMember(
                        dest => dest.EnginePower,
                        opt => opt.MapFrom(src => $"{src.EnginePower} (kW/hP)")
                    )
                    .ForMember(
                        dest => dest.AdNumber,
                        opt => opt.MapFrom(src => $"{src.UserId}-{src.Model}")
                    )
                    .ForMember(
                        dest => dest.HeadlinerImageUrl,
                        opt =>
                            opt.MapFrom(
                                src =>
                                    PhotoHelpers.ConvertFormFileToByteArray(src.HeadlinerImageUrl)
                            )
                    )
                    .ForMember(
                        dest => dest.PhotoOne,
                        opt =>
                            opt.MapFrom(
                                src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoOne)
                            )
                    )
                    .ForMember(
                        dest => dest.PhotoTwo,
                        opt =>
                            opt.MapFrom(
                                src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoTwo)
                            )
                    )
                    .ForMember(
                        dest => dest.PhotoThree,
                        opt =>
                            opt.MapFrom(
                                src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoThree)
                            )
                    )
                    .ForMember(
                        dest => dest.PhotoFour,
                        opt =>
                            opt.MapFrom(
                                src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoFour)
                            )
                    )
                    .ForMember(
                        dest => dest.PhotoFive,
                        opt =>
                            opt.MapFrom(
                                src => PhotoHelpers.ConvertFormFileToByteArray(src.PhotoFive)
                            )
                    );

                cfg.CreateMap<EditCarDto, EditCarViewModel>();

                cfg.CreateMap<EditCarViewModel, EditCarDto>()
                    .ForMember(dest => dest.UserId, opt => opt.Ignore());
                cfg.CreateMap<RegisterUserViewModel, RegisterUserDto>();
                cfg.CreateMap<LoginUserViewModel, LoginUserDto>();
                cfg.CreateMap<EditUserDto, EditUserViewModel>();
                cfg.CreateMap<EditUserViewModel, EditUserDto>()
                    .ForMember(
                        dest => dest.ProfilePictureImageUrl,
                        opt =>
                            opt.MapFrom(
                                src =>
                                    PhotoHelpers.ConvertFormFileToByteArray(
                                        src.ProfilePictureImageUrl
                                    )
                            )
                    );
            });
            _mapper = configuration.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
