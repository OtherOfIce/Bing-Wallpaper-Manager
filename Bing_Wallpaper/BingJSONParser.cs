using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace Bing_Wallpaper
{
    public class BingWallpapers
    {
        public List<string> imageIds { get; set; }
        public List<string> categories { get; set; }
        public List<string> tags { get; set; }
        public List<string> holidays { get; set; }
        public List<string> regions { get; set; }
        public List<string> countries { get; set; }
        public List<string> colors { get; set; }
        public List<string> shortNames { get; set; }
        public List<string> imageNames { get; set; }
        public List<int> dates { get; set; }
    }

    public class WallpaperInfo
    {
        public string title { get; set; }
        public string infoUrl { get; set; }
        public string infoLink { get; set; }
        public string similarLink { get; set; }
        public bool wallpaper { get; set; }
        public string wpFullFilename { get; set; }
        public string wpShortFilename { get; set; }
        public List<string> text { get; set; }
    }

    class BingJSONParser : IParser
    {
        public BingWallpapers allWallpapers;
        public List<WallpaperInfo> wallpaperDetailsList;
        private uint imagePosition = 0;

        public BingJSONParser()
        {
            wallpaperDetailsList = new List<WallpaperInfo>();
            using (WebClient client = new WebClient())
            {
                String jsonData = client.DownloadString(@"http://www.bing.com/gallery/home/browsedata?z=0");
                jsonData = jsonData.Remove(0, 48);
                jsonData = jsonData.Remove(jsonData.Length - 27, 27);
                allWallpapers =
                    JsonConvert.DeserializeObject<BingWallpapers>(jsonData
                    );

                Thread removeBadWallpapersThread = new Thread(new ThreadStart(removeNonWallpapers));
                removeBadWallpapersThread.IsBackground = true;
                removeBadWallpapersThread.Start();
            }
        }


        public void removeNonWallpapers()
        {
            BingWallpapers data = allWallpapers;
            for (int index = 0; index < allWallpapers.imageIds.Count; index++)
            {
                WallpaperInfo removeWallpaperInfo = JsonConvert.DeserializeObject<WallpaperInfo>(
                    new WebClient().DownloadString(
                        "http://www.bing.com/gallery/home/imagedetails/" + data.imageIds[index]));
                if (removeWallpaperInfo.wallpaper)
                {
                    wallpaperDetailsList.Add(removeWallpaperInfo);
                }
            }
        }

        public ImageDetails GetNextImageDetails()
        {
            WallpaperInfo wallInfo = wallpaperDetailsList[(int) imagePosition];

            ImageDetails details;
            details.ImageUri = new Uri("http://az608707.vo.msecnd.net/files/" + wallInfo.wpFullFilename);
            details.ImageFilePath = wallInfo.wpShortFilename;

            imagePosition += 1;
            return details;
        }

        public ImageDetails GetPreviousImageDetails()
        {
            if (imagePosition != 0)
            {
                imagePosition -= 1;
            }
            WallpaperInfo wallInfo = wallpaperDetailsList[(int) imagePosition];
            ImageDetails details;
            details.ImageUri = new Uri("http://az608707.vo.msecnd.net/files/" + wallInfo.wpFullFilename);
            details.ImageFilePath = wallInfo.wpShortFilename;
            return details;
        }

        public ImageDetails GetSpecificImageDetails(uint imgNum)
        {
            while (imgNum >= wallpaperDetailsList.Count)
            {
                Thread.Sleep(500);
            }
            WallpaperInfo wallInfo = wallpaperDetailsList[(int) imgNum];

            ImageDetails details;
            details.ImageUri = new Uri("http://az608707.vo.msecnd.net/files/" + wallInfo.wpFullFilename);
            details.ImageFilePath = wallInfo.wpShortFilename;

            return details;
        }

        public uint GetImageNumber()
        {
            throw new NotImplementedException();
        }

        public void SetImageNumber()
        {
            throw new NotImplementedException();
        }
    }
}