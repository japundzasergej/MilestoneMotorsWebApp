using Microsoft.AspNetCore.Http;
using MilestoneMotorsWebApp.Business.Cars.Commands;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

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

        private static List<byte[]> FilterNonNullByteArrays(params byte[][] byteArrays)
        {
            List<byte[]> result =  [ ];

            foreach (byte[] byteArray in byteArrays)
            {
                if (byteArray != null && byteArray.Length > 0)
                {
                    result.Add(byteArray);
                }
            }

            return result;
        }

        private async Task<List<string>> CloudinaryUpload(
            List<byte[]> byteArrayList,
            List<string> contentTypeList
        )
        {
            List<string> result =  [ ];

            for (int index = 0; index < byteArrayList.Count; index++)
            {
                var file = byteArrayList[index];

                if (file != null)
                {
                    using var memoryStream = new MemoryStream(file);
                    await memoryStream.WriteAsync(file);

                    var convertedFile = new FormFile(
                        memoryStream,
                        0,
                        memoryStream.Length,
                        "file",
                        $"image{index}"
                    )
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = contentTypeList[index]
                    };

                    var imageFile = await _photoService.AddPhotoAsync(convertedFile);
                    if (imageFile != null)
                    {
                        result.Add(imageFile.Url.ToString());
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
            var carDto = request.CreateCarDto;
            var imageServiceDto = new ImageServiceDto();

            List<byte[]> byteList = FilterNonNullByteArrays(
                carDto?.HeadlinerImageUrl,
                carDto?.PhotoOne,
                carDto?.PhotoTwo,
                carDto?.PhotoThree,
                carDto?.PhotoFour,
                carDto?.PhotoFive
            );

            List<string> contentTypeList = carDto.ImageContentTypes;

            List<string> imagesUrl = await CloudinaryUpload(byteList, contentTypeList);

            if (imagesUrl.Count == 0)
            {
                imageServiceDto.ImageServiceDown = true;
            }

            var car = _mapperService.Map<CreateCarDto, Car>(carDto);
            car.CreatedAt = DateTime.UtcNow;
            car.HeadlinerImageUrl = imagesUrl[0];
            var carImages = imagesUrl.Skip(1).ToList();
            car.ImagesUrl = carImages;

            var result = await _repository.Add(car);
            if (result)
            {
                return imageServiceDto;
            }

            imageServiceDto.DbSuccessful = false;
            return imageServiceDto;
        }
    }
}
