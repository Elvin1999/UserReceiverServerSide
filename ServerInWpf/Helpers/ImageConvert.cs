using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServerInWpf.Helpers
{
    public class ImageConvert
    {
        public static string GetImagePath(byte[] buffer)
        {
            // C: \Users\e.camalzade\source\repos\ServerInWpf\ServerInWpf\Images
            ImageConverter ic = new ImageConverter();
            Image img = (Image)ic.ConvertFrom(buffer);
            Bitmap bitmap1 = new Bitmap(img);
            var unique = Guid.NewGuid();
            string imagePath = "";
            bitmap1.Save($@"../../Images/image{unique}.jpg");
            imagePath = $@"../../Images/image{unique}.jpg";
            return imagePath;
        }
        public static byte[] GetBytesOfImage(string path)
        {
            var image = new Bitmap(path);
            ImageConverter imageconverter = new ImageConverter();
            var imagebytes = ((byte[])imageconverter.ConvertTo(image, typeof(byte[])));
            return imagebytes;
        }
    }
}
