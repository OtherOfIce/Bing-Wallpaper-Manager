using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;
using Timer = System.Timers.Timer;

namespace Bing_Wallpaper
{
    class Bing_Wallpaper_Manager
    {
        private BingJSONParser parser = new BingJSONParser();


        static String Base =
            Directory.GetCurrentDirectory() + "\\img\\" + "image";

        uint FileNumber = 0;
        static String FileExtention = ".bmp";
        public String fullPath;
        private MainWindow window;

        public Bing_Wallpaper_Manager(MainWindow window)
        {
            this.window = window;
            fullPath = Base + FileNumber + FileExtention;
            InitiliseDirectories();
            DownloadImage.Download(BingXMLParser.getImageURL(FileNumber), fullPath);
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            Timer timer = new Timer(2000);
            timer.Elapsed += new ElapsedEventHandler(cacheWallpapers);
            timer.Enabled = true;
            timer.Start();


        }

        public void cacheWallpapers(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Cacheing");
            for (uint i = FileNumber; i < FileNumber + 30; i++)
            {
                DownloadImage.Download(parser.getImageURL(i), Base + i + FileExtention);

            }
            
        }

        public void InitiliseDirectories()
        {

            List<String> pathsList = new List<string>{"\\img\\"};

            foreach (String path in pathsList)
            {
                String fullPath = Directory.GetCurrentDirectory() + path;
                if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, true);
                }
                Directory.CreateDirectory(fullPath);
            }
        }


        public void UpdateWallpaper()
        {
            fullPath = Base + FileNumber + FileExtention;
            DownloadImage.Download(parser.getImageURL(FileNumber), fullPath);
            BitmapImage image = new BitmapImage(new Uri(fullPath));
            window.ImageBackground.ImageSource = image;
        }

        public void NextWallpaper()
        {
            FileNumber++;
            UpdateWallpaper();
        }

        public void PreviousWallpaper()
        {
            if (FileNumber != 0)
            {
                FileNumber--;
                UpdateWallpaper();
            }
        }
    }
}