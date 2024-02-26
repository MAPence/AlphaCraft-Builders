using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ACB.Controllers
{
    public class ImageHandler
    {
        public static byte[] ConvertImageFile(IFormFile imageFile)
        {
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
            {return Image.FromStream(ms);}
        }   
    }
}