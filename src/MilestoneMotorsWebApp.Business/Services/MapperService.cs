using AutoMapper;
using Microsoft.AspNetCore.Http;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Services
{
    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper;

        public MapperService()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateCarDto, Car>();

                cfg.CreateMap<Car, EditCarDto>()
                    .ForMember(dest => dest.Price, opt => opt.Ignore())
                    .ForMember(dest => dest.EngineCapacity, opt => opt.Ignore())
                    .ForMember(dest => dest.EnginePower, opt => opt.Ignore())
                    .ForMember(dest => dest.Mileage, opt => opt.Ignore());

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
                cfg.CreateMap<User, EditUserDto>()
                    .ForMember(dest => dest.ProfilePictureImageUrl, opt => opt.Ignore());
            });
            _mapper = configuration.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
