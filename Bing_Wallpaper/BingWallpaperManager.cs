using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Media.Imaging;
using Timer = System.Timers.Timer;

namespace Bing_Wallpaper
{
    class BingWallpaperManager
    {
        private StreamWriter file = new StreamWriter("test.txt");
        private readonly BingJSONParser parser = new BingJSONParser(); //Default Parser
        private List<ImageDetails> imageList = new List<ImageDetails>(); //List of images

        static String basePath =
            Directory.GetCurrentDirectory() + "\\img\\"; //The path for the image!

        private DownloadImageManager downloadImageManager = new DownloadImageManager(); //Download manager

        public int imageNumber = 0; //What image are we looking at.
        public String fullPath; //The fullpath to the directory ?
        private MainWindow window;

        public BingWallpaperManager(MainWindow window)
        {
            this.window = window;
            InitiliseDirectories();

            ImageDetails temp = new BingXMLParser().GetSpecificImageDetails(0);
            imageList.Add(temp);
            downloadImageManager.DownloadImage(temp);
            UpdateWallpaper(temp);

            Thread chacheThread = new Thread(new ThreadStart(cacheWallpapers));
            chacheThread.IsBackground = true;
            chacheThread.Start();
        }

        public void cacheWallpapers()
        {
            Console.WriteLine("Starting caching");
            int cacheNumber = 0;
            while (true)
            {
                for (;cacheNumber < imageNumber + 10; cacheNumber++)
                {
                    ImageDetails details = parser.GetSpecificImageDetails((uint)cacheNumber);
                    String ImagePath = basePath + details.ImageFilePath;
                    Console.WriteLine("Cache " + cacheNumber + " File Name: " + details.ImageFilePath);
                    downloadImageManager.DownloadImage(details);
                    imageList.Add(details);
                }
            }
        }

        public void InitiliseDirectories()
        {
            List<String> pathsList = new List<string> {"\\img\\"};

            foreach (String path in pathsList)
            {
                String fullPath = Directory.GetCurrentDirectory() + path;
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
            }
        }


        public void UpdateWallpaper(ImageDetails details)
        {
            fullPath = basePath + details.ImageFilePath;
            downloadImageManager.DownloadImage(details);
            BitmapImage image = new BitmapImage(new Uri(fullPath));
            window.ImageBackground.ImageSource = image;
        }

        public void NextWallpaper()
        {
            
            while (!(imageList.Count >= imageNumber + 2))
            {
            }

            imageNumber++;
            UpdateWallpaper(imageList[imageNumber]);
        }

        public void PreviousWallpaper()
        {
            if (imageNumber > 0)
            {
                imageNumber--;
                UpdateWallpaper(imageList[imageNumber]);
            }
        }
    }
}