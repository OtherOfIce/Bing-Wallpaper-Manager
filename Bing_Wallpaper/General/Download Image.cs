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
        private static void DownloadFullImage(ImageDetails details)
        {
            FileInfo file = new FileInfo(details.ImageFilePath);
            using (WebClient client = new WebClient())
            {
                if (file.Exists && file.Length == 0)
                {
                    file.Delete();
                }

                if (!file.Exists)
                {
                    client.DownloadFile(details.ImageUri, details.ImageFilePath);
                }
            }
        }


        private static void DownloadThumbnail(ImageDetails details)
        {
            FileInfo file = new FileInfo(details.ThumbnailFilePath);
            using (WebClient client = new WebClient())
            {
                if (file.Exists && file.Length == 0)
                {
                    file.Delete();
                }

                if (!file.Exists)
                {
                    client.DownloadFile(new Uri(
                            details.ImageUri.OriginalString.Remove(details.ImageUri.OriginalString.Length - 13, 13) +
                            "320x180.jpg"),
                        details.ThumbnailFilePath); //Relace 1920x1080.jpg with 320x180.jpg then download
                }
            }
        }


        public static void DownloadImage(ImageDetails details)
        {
            DownloadFullImage(details);
            DownloadThumbnail(details);
        }
    }
}