using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Change_Wallpaper;

namespace Bing_Wallpaper
{
    public partial class Home_Page : ISwitchable
    {
        public readonly BingWallpaperManager manager;

        public Home_Page(BingWallpaperManager man)
        {
            InitializeComponent();
            manager = man;
            this.DataContext = manager;
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