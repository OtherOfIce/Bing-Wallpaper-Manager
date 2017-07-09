using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using Bing_Wallpaper.Annotations;
using Bing_Wallpaper.Pages.Time_Line_Page;

namespace Bing_Wallpaper
{
    public class BingWallpaperManager : INotifyPropertyChanged
    {
        private readonly BingJSONParser _defaultParser = new BingJSONParser();
        public readonly List<ImageDetails> ImageList = new List<ImageDetails>();
        public readonly ObservableCollection<Thumbnail> Thumbnails = new ObservableCollection<Thumbnail>();



        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public int ImageNumber;

        public BingWallpaperManager()
        {
            InitiliseDirectories();


            var temp = new BingXMLParser().GetSpecificImageDetails(0);
            DownloadImageManager.DownloadImage(temp);
            ImageList.Add(temp);
            UpdateWallpaper(temp);

            new Thread(CacheWallpapers) {IsBackground = true}.Start();
            new Thread(CacheThumbnails) { IsBackground = true }.Start();

        }

        public void CacheWallpapers()
        {
            var cacheNumber = 0;
            while (true)
                for (; cacheNumber < ImageNumber + 10; cacheNumber++)
                {
                    var details = _defaultParser.GetSpecificImageDetails((uint) cacheNumber);
                    DownloadImageManager.DownloadImage(details);
                    ImageList.Add(details);
                    

                }
        }

        public void CacheThumbnails()
        {
            var cacheNumber = 0;
            for (; cacheNumber < 200; cacheNumber++)
            {
                var details = _defaultParser.GetSpecificImageDetails((uint) cacheNumber);
                
                lock (ImageList)
                {
                    if (!ImageList.Contains(details))
                    {
                        ImageList.Add(details);
                        DownloadImageManager.DownloadImage(details);
                    }
                }
            
                    
                Application.Current.Dispatcher.Invoke(() => Thumbnails.Add(new Thumbnail(new BitmapImage(
                    new Uri(details.ThumbnailFilePath)))));
            }
        }

        public void InitiliseDirectories()
        {
            var pathsList = new List<string> {"\\img\\", "\\json\\", "\\thumbnails\\"};

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
            CurrentImage =
                new BitmapImage(new Uri(details.ImageFilePath));
        }

        public void NextWallpaper()
        {
            while (!(ImageList.Count >= ImageNumber + 2))
            {
            }

            ImageNumber++;
            UpdateWallpaper(ImageList[ImageNumber]);
        }

        public void PreviousWallpaper()
        {
            if (ImageNumber > 0)
            {
                ImageNumber--;
                UpdateWallpaper(ImageList[ImageNumber]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}