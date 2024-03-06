using MediatR;
using Microsoft.AspNetCore.Http;
using MilestoneMotorsWebApp.Business.Cars.Helpers;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class CreateCarCommandHandler(
        ICarsRepository carsRepository,
        IPhotoService photoService,
        IMapperService mapperService
    ) : IRequestHandler<CreateCarCommand, ImageServiceDto>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<ImageServiceDto> Handle(
            CreateCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var carDto = request.CreateCarDto;
            var imageServiceDto = new ImageServiceDto();

            List<byte[]> byteList = FilterBytes.FilterNonNullByteArrays(
                carDto?.HeadlinerImageUrl,
                carDto?.PhotoOne,
                carDto?.PhotoTwo,
                carDto?.PhotoThree,
                carDto?.PhotoFour,
                carDto?.PhotoFive
            );

            List<string> contentTypeList = carDto.ImageContentTypes;

            List<string> imagesUrl = (List<string>?)
                await _photoService.CloudinaryUpload(byteList, contentTypeList);

            if (imagesUrl == null || imagesUrl.Count == 0)
            {
                imageServiceDto.ImageServiceDown = true;
            }

            var car = _mapperService.Map<CreateCarDto, Car>(carDto);
            car.CreatedAt = DateTime.UtcNow;
            car.HeadlinerImageUrl = imagesUrl[0] ?? "";
            var carImages = imagesUrl.Skip(1).ToList();
            car.ImagesUrl = carImages;

            var result = await _carsRepository.Add(car);
            if (result)
            {
                return imageServiceDto;
            }

            imageServiceDto.DbSuccessful = false;
            return imageServiceDto;
        }
    }
}
