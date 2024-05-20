using AutoMapper;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Mapper
{
    public class CarMapperProfile : Profile
    {
        public CarMapperProfile()
        {
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<CreateCarDto, Car>();

            CreateMap<Car, EditCarDto>();

            CreateMap<EditCarDto, Car>()
                .ForMember(dest => dest.HeadlinerImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ImagesUrl, opt => opt.Ignore());
        }
    }
}
