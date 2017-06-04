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


        public void DownloadImage(ImageDetails details)
        {
            using (WebClient client = new WebClient())
            {
                String fullPath = Directory.GetCurrentDirectory() + "\\img\\" + details.ImageFilePath;
                if (!File.Exists(fullPath))
                {
                    client.DownloadFile(details.ImageUri, fullPath);
                }
            }
        }

    }
}