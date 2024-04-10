using MediatR;
using MilestoneMotorsWebApp.Business.Cars.Helpers;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class CreateCarCommandHandler(
        ICarsRepository carsRepository,
        IPhotoService photoService,
        IMapperService mapperService
    ) : IRequestHandler<CreateCarCommand, ResponseDTO>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<ResponseDTO> Handle(
            CreateCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var carDto = request.CreateCarDto;
            var imageServiceDto = new ImageServiceDto();

            try
            {
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
                    return PopulateResponseDto.OnSuccess(imageServiceDto, 201);
                }

                imageServiceDto.DbSuccessful = false;
                return PopulateResponseDto.OnFailure(400, body: imageServiceDto);
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
