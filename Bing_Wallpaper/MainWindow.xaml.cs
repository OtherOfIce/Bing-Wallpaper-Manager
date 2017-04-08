using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Bing_Wallpaper
{
    public partial class MainWindow : Window
    {
        private BingJSONParser parser = new BingJSONParser();


        static String Base =
            Directory.GetCurrentDirectory() + "\\img\\" + "image";


        uint FileNumber = 0;
        static String FileExtention = ".bmp";
        static String fullPath;

        public MainWindow()
        {
            fullPath = Base + FileNumber + FileExtention;
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\img\\");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\json\\");
            DownloadImage.Download(BingXMLParser.getImageURL(FileNumber), fullPath);
            InitializeComponent();
            UpdateWallpaper();
        }


        private void UpdateWallpaper()
        {
            fullPath = Base + FileNumber + FileExtention;
            DownloadImage.Download(parser.getImageURL(FileNumber), fullPath);
            BitmapImage image = new BitmapImage(new Uri(fullPath));
            this.ImageBackground.ImageSource = image;
        }

        private void NextWallpaper()
        {
            FileNumber++;
        }

        private void PreviousWallpaper()
        {
            if (FileNumber != 0)
            {
                FileNumber--;
            }
        }

        private void Forwards_Button_Click(object sender, RoutedEventArgs e)
        {
            NextWallpaper();
            UpdateWallpaper();
        }

        private void Backwards_Button_Click(object sender, RoutedEventArgs e)
        {
            PreviousWallpaper();
            UpdateWallpaper();
        }

        private void Change_Wallpaper_Button_Click(object sender, RoutedEventArgs e)
        {
            Change_Wallpaper.ChangeWallpaper.SetBackground(fullPath);
        }
    }
}