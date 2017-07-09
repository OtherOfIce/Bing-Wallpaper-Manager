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
                

                String test = details.ImageUri.OriginalString;
                Uri thumbnailUri = new Uri(test.Remove(test.Length - 13, 13) + "320x180.jpg");
                

                if (!File.Exists(details.ImageFilePath))
                {
                    client.DownloadFile(details.ImageUri, details.ImageFilePath);
                }
                if (!File.Exists(details.ThumbnailFilePath))
                {
                    client.DownloadFile(thumbnailUri, details.ThumbnailFilePath);
                }
            }
        }

    }
}