using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Change_Wallpaper;

namespace Bing_Wallpaper
{
    public partial class Home_Page : ISwitchable
    {
        public HomeWallpaperManager manager;


        public Home_Page()
        {
            InitializeComponent();
            manager = new HomeWallpaperManager(this);
        }

        

        private void Forwards_Button_Click(object sender, RoutedEventArgs e)
        {
            manager.NextWallpaper();
        }

        private void Backwards_Button_Click(object sender, RoutedEventArgs e)
        {
            manager.PreviousWallpaper();
        }

        private void Change_Wallpaper_Button_Click(object sender, RoutedEventArgs e)
        {
            
            ChangeWallpaper.SetBackground(new Uri(ImageBackground.ImageSource.ToString()).LocalPath);
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}