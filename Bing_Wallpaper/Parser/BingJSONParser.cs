using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
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

    internal class BingJSONParser : IParser
    {
        private readonly BingWallpapers _allWallpapers;
        private readonly List<WallpaperInfo> _wallpaperDetailsList;
        private uint _imagePosition;

        public BingJSONParser()
        {
            _wallpaperDetailsList = new List<WallpaperInfo>();
            using (var client = new WebClient())
            {
                var jsonData = client.DownloadString(@"http://www.bing.com/gallery/home/browsedata?z=0");
                jsonData = jsonData.Remove(0, 48);
                jsonData = jsonData.Remove(jsonData.Length - 27, 27);
                _allWallpapers =
                    JsonConvert.DeserializeObject<BingWallpapers>(jsonData
                    );

                new Thread(RemoveNonWallpapers) {IsBackground = true}.Start();
            }
        }

        public ImageDetails GetNextImageDetails()
        {
            var wallInfo = _wallpaperDetailsList[(int) _imagePosition];

            ImageDetails details;
            details.ImageUri = new Uri("http://az608707.vo.msecnd.net/files/" + wallInfo.wpFullFilename);
            details.ImageFilePath = Directory.GetCurrentDirectory() + "\\img\\" + wallInfo.wpShortFilename;
            details.ThumbnailFilePath = Directory.GetCurrentDirectory() + "\\thumbnails\\" + wallInfo.wpShortFilename;
            _imagePosition += 1;
            return details;
        }

        public ImageDetails GetPreviousImageDetails()
        {
            if (_imagePosition != 0)
                _imagePosition -= 1;
            var wallInfo = _wallpaperDetailsList[(int) _imagePosition];
            ImageDetails details;
            details.ImageUri = new Uri("http://az608707.vo.msecnd.net/files/" + wallInfo.wpFullFilename);
            details.ImageFilePath = Directory.GetCurrentDirectory() + "\\img\\" + wallInfo.wpShortFilename;
            details.ThumbnailFilePath = Directory.GetCurrentDirectory() + "\\thumbnails\\" + wallInfo.wpShortFilename;
            return details;
        }

        public ImageDetails GetSpecificImageDetails(uint imgNum)
        {
            while (imgNum >= _wallpaperDetailsList.Count)
                Thread.Sleep(500);
            var wallInfo = _wallpaperDetailsList[(int) imgNum];

            ImageDetails details;
            details.ImageUri = new Uri("http://az608707.vo.msecnd.net/files/" + wallInfo.wpFullFilename);
            details.ImageFilePath = Directory.GetCurrentDirectory() + "\\img\\" + wallInfo.wpShortFilename;
            details.ThumbnailFilePath = Directory.GetCurrentDirectory() + "\\thumbnails\\" + wallInfo.wpShortFilename;

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


        private void RemoveNonWallpapers()
        {
            var data = _allWallpapers;
            for (var index = 0; index < _allWallpapers.imageIds.Count; index++)
            {
                var removeWallpaperInfo = JsonConvert.DeserializeObject<WallpaperInfo>(
                    new WebClient().DownloadString(
                        "http://www.bing.com/gallery/home/imagedetails/" + data.imageIds[index]));
                if (removeWallpaperInfo.wallpaper)
                    _wallpaperDetailsList.Add(removeWallpaperInfo);
            }
        }
    }
}