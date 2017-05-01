using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Media.Imaging;
using Timer = System.Timers.Timer;

namespace Bing_Wallpaper
{
    class BingWallpapWallpaperManager
    {
        private readonly BingJSONParser parser = new BingJSONParser(); //Default Parser
        private List<ImageDetails> imageList = new List<ImageDetails>();
        private int cacheNumber = 0;

        static String basePath =
            Directory.GetCurrentDirectory() + "\\img\\";

        private DownloadImageManager downloadImageManager = new DownloadImageManager(basePath);

        public int imageNumber = 0;
        public String fullPath;
        private MainWindow window;

        public BingWallpapWallpaperManager(MainWindow window)
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
            while (true)
            {
                for (int i = cacheNumber; cacheNumber < imageNumber + 20; i++)
                {
                    try
                    {

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    ImageDetails details = parser.GetSpecificImageDetails((uint) i);
                    String ImagePath = basePath + details.ImageFilePath;
                    Console.WriteLine("Cache " + i + " File Name: " + details.ImageFilePath);
                    imageList.Add(details);
                    downloadImageManager.DownloadImage(details);
                    cacheNumber++;
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
            try
            {
                imageNumber++;
                UpdateWallpaper(imageList[imageNumber]);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine("Out of range exception!");
                imageNumber--;
                NextWallpaper();
            }
            
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