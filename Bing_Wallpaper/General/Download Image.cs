using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bing_Wallpaper
{


    class DownloadImageManager
    {


        public static void DownloadImage(ImageDetails details)
        {
            using (WebClient client = new WebClient())
            {
                String ImagePath = Directory.GetCurrentDirectory() + "\\img\\" + details.ImageFilePath;

                String test = details.ImageUri.OriginalString;
                test = test.Remove(test.Length - 13, 13) + "320x180.jpg";
                String ThumbnailPath = Directory.GetCurrentDirectory() + "\\thumbnails\\" + details.ImageFilePath;

                if (!File.Exists(ImagePath))
                {
                    client.DownloadFile(details.ImageUri, ImagePath);
                }
                if (!File.Exists(ThumbnailPath))
                {
                    client.DownloadFile(test, ThumbnailPath);
                }
            }
        }

    }
}