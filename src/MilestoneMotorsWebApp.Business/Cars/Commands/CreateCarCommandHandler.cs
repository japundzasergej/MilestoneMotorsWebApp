﻿using AutoMapper;
using MediatR;
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
        IMapper mapper
    ) : IRequestHandler<CreateCarCommand, ImageServiceDto>
    {
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
                await photoService.CloudinaryUpload(byteList, contentTypeList);

            if (imagesUrl == null || imagesUrl.Count == 0)
            {
                imageServiceDto.ImageServiceDown = true;
            }

            var car = mapper.Map<Car>(carDto);
            car.HeadlinerImageUrl = imagesUrl[0] ?? "";
            var carImages = imagesUrl.Skip(1).ToList();
            car.ImagesUrl = carImages;

            await carsRepository.Add(car);

            return imageServiceDto;
        }
    }
}
