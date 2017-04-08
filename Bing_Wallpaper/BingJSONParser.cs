using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
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

    class BingJSONParser
    {
        private BingWallpapers wallpapers;

        public BingJSONParser()
        {
            using (WebClient client = new WebClient())
            {
                String jsonData = client.DownloadString(@"http://www.bing.com/gallery/home/browsedata?z=0");
                jsonData = jsonData.Remove(0, 48);
                jsonData = jsonData.Remove(jsonData.Length - 27, 27);
                wallpapers =
                    JsonConvert.DeserializeObject<BingWallpapers>(jsonData
                    );
                
            }
        }

        
        private void removeNonWallpapers(int index)
        {
            
                WallpaperInfo removeWallpaperInfo = JsonConvert.DeserializeObject<WallpaperInfo>(new WebClient().DownloadString(
                        "http://www.bing.com/gallery/home/imagedetails/" + wallpapers.imageIds[index]));
            
                    wallpapers.imageIds.RemoveAt(index);
                    wallpapers.categories.RemoveAt(index);
                    wallpapers.tags.RemoveAt(index);
                    wallpapers.holidays.RemoveAt(index);
                    wallpapers.regions.RemoveAt(index);
                    wallpapers.countries.RemoveAt(index);
                    wallpapers.colors.RemoveAt(index);
                    wallpapers.shortNames.RemoveAt(index);
                    wallpapers.imageNames.RemoveAt(index);
                    wallpapers.dates.RemoveAt(index);
        }
            
            
        

        public String getImageURL(uint imageNumber)
        {
            String imageID = wallpapers.imageIds[(int) imageNumber];
            String picDetails = "http://www.bing.com/gallery/home/imagedetails/" + imageID;

            using (WebClient client = new WebClient())
            {
                WallpaperInfo wallInfo =
                    JsonConvert.DeserializeObject<WallpaperInfo>(client.DownloadString(picDetails));

                while (true)
                {
                    if (wallInfo.wpFullFilename != "")
                    {
                        return ("http://az608707.vo.msecnd.net/files/" + wallInfo.wpFullFilename);
                    }
                    else
                    {
                        removeNonWallpapers((int)imageNumber);
                        return getImageURL((imageNumber));
                    }
                }
                
            }
        }
    }
}