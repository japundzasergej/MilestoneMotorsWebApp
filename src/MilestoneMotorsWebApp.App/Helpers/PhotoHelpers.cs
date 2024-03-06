namespace MilestoneMotorsWebApp.App.Helpers
{
    public static class PhotoHelpers
    {
        public static List<string> GetImageContentType(List<IFormFile> imageFiles)
        {
            List<string> imageContentTypeList =  [ ];
            foreach (var image in imageFiles)
            {
                if (image != null)
                {
                    imageContentTypeList.Add(image.ContentType);
                }
            }
            return imageContentTypeList;
        }

        public static byte[]? ConvertFormFileToByteArray(IFormFile? file)
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
