using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bing_Wallpaper
{
    class DownloadImage
    {

        public static String Download(String url, String downloadPath)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), downloadPath);
            }
            return downloadPath;
        }
    }
}
