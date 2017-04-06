using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Bing_Wallpaper
{
    public partial class MainWindow : Window
    {
        static String Base =
            @"F:\Libraries\Documents\Visual Studio 2017\Projects\Bing_Wallpaper\Bing_Wallpaper\image";

        uint FileNumber = 0;
        static String FileExtention = ".bmp";
        static String fullPath;

        public MainWindow()
        {
            fullPath = Base + FileNumber + FileExtention;
            DownloadImage.Download(BingXMLParser.getImageURL(FileNumber), fullPath);
            InitializeComponent();
        }

        private void Forwards_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Foward button clicked!");
            FileNumber++;
            UpdateWallpaper();
        }

        private void Backwards_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Backwards button clicked!");
            FileNumber--;
            UpdateWallpaper();
        }


        private void UpdateWallpaper()
        {
            Console.WriteLine(fullPath);
            fullPath = Base + FileNumber + FileExtention;

            if (!File.Exists(fullPath))
            {
                DownloadImage.Download(BingXMLParser.getImageURL(FileNumber),fullPath);               
            }
            BitmapImage image = new BitmapImage(new Uri(fullPath));
            this.WallpaperImage.Source = image;

        }

        private void Change_Wallpaper_Button_Click(object sender, RoutedEventArgs e)
        {
            Change_Wallpaper.ChangeWallpaper.SetBackground(
                DownloadImage.Download(BingXMLParser.getImageURL(FileNumber), fullPath));
        }
    }
}