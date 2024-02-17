using AutoMapper;
using Microsoft.AspNetCore.Http;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Common.Utilities;
using MilestoneMotorsWebApp.Common.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Common.Services
{
    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper;

        public MapperService()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Car, CreateCarViewModel>();
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
                        opt => opt.MapFrom(src => ConvertFormFileToByteArray(src.HeadlinerImageUrl))
                    )
                    .ForMember(
                        dest => dest.PhotoOne,
                        opt => opt.MapFrom(src => ConvertFormFileToByteArray(src.PhotoOne))
                    )
                    .ForMember(
                        dest => dest.PhotoTwo,
                        opt => opt.MapFrom(src => ConvertFormFileToByteArray(src.PhotoTwo))
                    )
                    .ForMember(
                        dest => dest.PhotoThree,
                        opt => opt.MapFrom(src => ConvertFormFileToByteArray(src.PhotoThree))
                    )
                    .ForMember(
                        dest => dest.PhotoFour,
                        opt => opt.MapFrom(src => ConvertFormFileToByteArray(src.PhotoFour))
                    )
                    .ForMember(
                        dest => dest.PhotoFive,
                        opt => opt.MapFrom(src => ConvertFormFileToByteArray(src.PhotoFive))
                    );

                cfg.CreateMap<CreateCarDto, Car>();

                cfg.CreateMap<Car, EditCarDto>()
                    .ForMember(dest => dest.Price, opt => opt.Ignore())
                    .ForMember(dest => dest.EngineCapacity, opt => opt.Ignore())
                    .ForMember(dest => dest.EnginePower, opt => opt.Ignore())
                    .ForMember(dest => dest.Mileage, opt => opt.Ignore());

                cfg.CreateMap<EditCarDto, EditCarViewModel>();

                cfg.CreateMap<EditCarViewModel, EditCarDto>()
                    .ForMember(dest => dest.UserId, opt => opt.Ignore());

                cfg.CreateMap<EditCarDto, Car>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.UserId, opt => opt.Ignore())
                    .ForMember(dest => dest.HeadlinerImageUrl, opt => opt.Ignore())
                    .ForMember(dest => dest.ImagesUrl, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.AdNumber, opt => opt.Ignore())
                    .ForMember(
                        dest => dest.Price,
                        opt => opt.MapFrom(src => ConvertToEuroMethod.ConvertToEuro(src.Price))
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
                        opt => opt.MapFrom(src => $"{src.EnginePower} (kW/hP) ")
                    );
                cfg.CreateMap<RegisterUserViewModel, RegisterUserDto>();
                cfg.CreateMap<LoginUserViewModel, LoginUserDto>();
                cfg.CreateMap<User, EditUserDto>()
                    .ForMember(dest => dest.ProfilePictureImageUrl, opt => opt.Ignore());
                cfg.CreateMap<EditUserDto, EditUserViewModel>();
                cfg.CreateMap<EditUserViewModel, EditUserDto>()
                    .ForMember(
                        dest => dest.ProfilePictureImageUrl,
                        opt =>
                            opt.MapFrom(
                                src => ConvertFormFileToByteArray(src.ProfilePictureImageUrl)
                            )
                    );
            });
            _mapper = configuration.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        private static byte[]? ConvertFormFileToByteArray(IFormFile? file)
        {
            if (file != null)
            {
                using MemoryStream memoryStream = new();
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
            return null;
        }
    }
}
