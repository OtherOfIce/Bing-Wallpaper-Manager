using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Bing_Wallpaper
{
    public class HomeWallpaperManager
    {
        private readonly BingJSONParser _defaultParser = new BingJSONParser();
        private readonly List<ImageDetails> _imageList = new List<ImageDetails>();
        private readonly Home_Page _window;
        
        public int ImageNumber;

        public HomeWallpaperManager(Home_Page window)
        {
            _window = window;
            InitiliseDirectories();

            
            var temp = new BingXMLParser().GetSpecificImageDetails(0);
            _imageList.Add(temp);
            DownloadImageManager.DownloadImage(temp);
            UpdateWallpaper(temp);

            new Thread(CacheWallpapers) {IsBackground = true}.Start();
        }

        public void CacheWallpapers()
        {
            var cacheNumber = 0;
            while (true)
                for (; cacheNumber < ImageNumber + 10; cacheNumber++)
                {
                    var details = _defaultParser.GetSpecificImageDetails((uint) cacheNumber);
                    DownloadImageManager.DownloadImage(details);
                    _imageList.Add(details);
                }
        }

        public void InitiliseDirectories()
        {
            var pathsList = new List<string> {"\\img\\", "\\json\\"};

            foreach (var path in pathsList)
            {
                var fullPath = Directory.GetCurrentDirectory() + path;
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
            }
        }


        public void UpdateWallpaper(ImageDetails details)
        {
            DownloadImageManager.DownloadImage(details);
            var image = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\img\\" + details.ImageFilePath));
            _window.ImageBackground.ImageSource = image;
        }

        public void NextWallpaper()
        {
            while (!(_imageList.Count >= ImageNumber + 2))
            {
            }

            ImageNumber++;
            UpdateWallpaper(_imageList[ImageNumber]);
        }

        public void PreviousWallpaper()
        {
            if (ImageNumber > 0)
            {
                ImageNumber--;
                UpdateWallpaper(_imageList[ImageNumber]);
            }
        }
    }
}