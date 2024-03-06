using System.Collections;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;

namespace MilestoneMotorsWebApp.Business.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary? _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            try
            {
                _cloudinary = new Cloudinary(account);
            }
            catch (Exception)
            {
                _cloudinary = null;
            }
        }

        public async Task<ImageUploadResult?> AddPhotoAsync(IFormFile file)
        {
            if (_cloudinary != null)
            {
                var uploadResult = new ImageUploadResult();
                if (file.Length > 0)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream)
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                return uploadResult;
            }
            return null;
        }

        public async Task<DeletionResult?> DeletePhotoAsync(string publicUrl)
        {
            if (_cloudinary != null)
            {
                var publicId = publicUrl.Split('/').Last().Split('.')[0];
                var deleteParams = new DeletionParams(publicId);
                return await _cloudinary.DestroyAsync(deleteParams);
            }
            return null;
        }

        public async Task<object?> CloudinaryUpload(object? bytes, object content)
        {
            if (bytes is byte[] b && content is string c)
            {
                if (bytes != null && b.Length > 0)
                {
                    using var memoryStream = new MemoryStream(b);
                    await memoryStream.WriteAsync(b);

                    var convertedFile = new FormFile(
                        memoryStream,
                        0,
                        memoryStream.Length,
                        "file",
                        "profilePicture"
                    )
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = c
                    };

                    var result = await AddPhotoAsync(convertedFile);
                    return result?.Url.ToString() ?? "";
                }
                else
                {
                    return null;
                }
            }
            else if (bytes is List<byte[]> bl && content is List<string> sl)
            {
                List<string> result =  [ ];

                for (int index = 0; index < bl.Count; index++)
                {
                    var file = bl[index];

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
                            ContentType = sl[index]
                        };

                        var imageFile = await AddPhotoAsync(convertedFile);
                        if (imageFile != null)
                        {
                            result.Add(imageFile.Url.ToString());
                        }
                    }
                }

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
