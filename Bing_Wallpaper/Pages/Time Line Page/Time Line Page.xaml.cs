using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            ThumbnailControl.ItemsSource = manager._thumbnails;
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem) sender;
            ContextMenu help = (ContextMenu) item.CommandParameter;
            Image image = (Image) help.PlacementTarget;
            Console.WriteLine(Directory.GetCurrentDirectory() + "//img//" + System.IO.Path.GetFileName(image.Source.ToString()));
            Change_Wallpaper.ChangeWallpaper.SetBackground(Directory.GetCurrentDirectory() + "\\img\\" + System.IO.Path.GetFileName(image.Source.ToString()));
        }

    }
}
