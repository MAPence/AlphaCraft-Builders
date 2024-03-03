using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ACB.Controllers
{
    public class ImageHandler
    {
        public static byte[] ConvertImageFile(IFormFile imageFile)
        {
            if (!IsSupportedImageType(imageFile.ContentType))
            {
                throw new NotSupportedException("Unsupported image type. Please upload a valid image file.");
            }

            byte[]? image = null;

            if (imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    image = ms.ToArray();
                }
            }
            return image;
        }

        public static Image ConvertByteArrayToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }

        public static bool IsSupportedImageType(string contentType)
        {
            return contentType.StartsWith("image/");
        }
    }
}