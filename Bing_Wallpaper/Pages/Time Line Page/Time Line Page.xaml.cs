using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Bing_Wallpaper.Pages.Time_Line_Page
{
    public class Thumbnail
    {
        public Thumbnail(BitmapImage image)
        {
            this.image = image;
        }

        public BitmapImage image { get; set; }
    }

    public partial class Time_Line_Page : UserControl, ISwitchable
    {
        public readonly BingWallpaperManager manager;


        public Time_Line_Page(BingWallpaperManager man)
        {
            manager = man;
            InitializeComponent();
            ThumbnailControl.ItemsSource = manager.Thumbnails;
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void MenuItem_Set_Wallpaper_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem) sender;
            ContextMenu help = (ContextMenu) item.CommandParameter;
            Image image = (Image) help.PlacementTarget;
            Change_Wallpaper.ChangeWallpaper.SetBackground(Directory.GetCurrentDirectory() + "\\img\\" +
                                                           System.IO.Path.GetFileName(image.Source.ToString()));
        }

        private void MenuItem_View_Wallpaper_OnClick(object sender, RoutedEventArgs e)
        {

            MenuItem item = (MenuItem) sender;
            ContextMenu help = (ContextMenu) item.CommandParameter;
            Image image = (Image) help.PlacementTarget;
            int imagePos = ThumbnailControl.Items.IndexOf(image.DataContext) + 1;
            manager.ImageNumber = imagePos;
            manager.UpdateWallpaper(manager.ImageList[imagePos]);
            ((PageSwitcher) Application.Current.MainWindow).SwitchHomepage();
            ((PageSwitcher) Application.Current.MainWindow).HomeButton.IsSelected = true;
        }
    }
}
