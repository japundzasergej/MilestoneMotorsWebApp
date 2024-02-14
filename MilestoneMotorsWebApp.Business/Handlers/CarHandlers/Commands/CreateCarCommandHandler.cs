using Microsoft.AspNetCore.Http;
using MilestoneMotorsWebApp.Business.Cars.Commands;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;
using MilestoneMotorsWebApp.Infrastructure.Repositories;

namespace MilestoneMotorsWebApp.Business.Handlers.CarHandlers.Commands
{
    public class CreateCarCommandHandler(
        ICarsRepository carsRepository,
        IPhotoService photoService,
        IMapperService mapperService
    )
        : BaseHandler<CreateCarCommand, ImageServiceDto, ICarsRepository>(
            carsRepository,
            mapperService
        )
    {
        private readonly IPhotoService _photoService = photoService;

        private async Task<List<string>> CloudinaryUpload(List<IFormFile?> files)
        {
            List<string> result =  [ ];
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var imageFile = await _photoService.AddPhotoAsync(file);
                        if (imageFile != null)
                        {
                            result.Add(imageFile.Url.ToString());
                        }
                    }
                }
            }
            return result;
        }

        public override async Task<ImageServiceDto> Handle(
            CreateCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var carDto = request.CarDto;
            var imageServiceDto = new ImageServiceDto();
            string? headlinerImage;
            if (carDto.HeadlinerImageUrl != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(carDto.HeadlinerImageUrl);
                if (photoResult != null)
                {
                    headlinerImage = photoResult.Url.ToString();
                }
                else
                {
                    imageServiceDto.ImageServiceDown = true;
                    headlinerImage = null;
                }
            }
            else
            {
                headlinerImage = null;
            }

            List<IFormFile?> files =
            [
                carDto?.PhotoOne,
                carDto?.PhotoTwo,
                carDto?.PhotoThree,
                carDto?.PhotoFour,
                carDto?.PhotoFive,
            ];

            List<string> imagesUrl = await CloudinaryUpload(files) ?? [ ];

            if (imagesUrl.Count == 0)
            {
                imageServiceDto.ImageServiceDown = true;
            }

            var car = _mapperService.Map<CreateCarDto, Car>(carDto);
            car.HeadlinerImageUrl = headlinerImage ?? "";
            car.ImagesUrl = imagesUrl;

            var result = await _repository.Add(car);

            return imageServiceDto;
        }
    }
}
