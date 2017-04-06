using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace JSON_PARSING
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

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (WebClient client = new WebClient())
            {
                String jsonData = client.DownloadString(@"http://www.bing.com/gallery/home/browsedata?z=0");
                jsonData = jsonData.Remove(0, 48);
                jsonData = jsonData.Remove(jsonData.Length - 27, 27);
                Console.WriteLine(jsonData);
                BingWallpapers r =
                    JsonConvert.DeserializeObject<BingWallpapers>(jsonData
                        );

                for (int i = 0; i < 30; i++)
                {
                    String imageID = r.imageIds[i];
                    String picDetails = "http://www.bing.com/gallery/home/imagedetails/" + imageID;

                    WallpaperInfo wallInfo =
                    JsonConvert.DeserializeObject<WallpaperInfo>(client.DownloadString(picDetails));
                    
                    if (wallInfo.wpFullFilename != "")
                    {
                        Debug.WriteLine(picDetails);
                        Debug.WriteLine(wallInfo.wpFullFilename);
                        Debug.WriteLine(wallInfo.title);
                        client.DownloadFile("http://az608707.vo.msecnd.net/files/" + wallInfo.wpFullFilename, wallInfo.wpFullFilename);
                    }
                }
            }
        }
    }
}