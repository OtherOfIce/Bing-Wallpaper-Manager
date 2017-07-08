using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media.Imaging;
using Bing_Wallpaper.Annotations;
using Bing_Wallpaper.Pages.Time_Line_Page;

namespace Bing_Wallpaper
{
    public class BingWallpaperManager : INotifyPropertyChanged
    {
        private readonly BingJSONParser _defaultParser = new BingJSONParser();
        public readonly List<ImageDetails> _imageList = new List<ImageDetails>();
        public ObservableCollection<Thumbnail> _thumbnails = new ObservableCollection<Thumbnail>();



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

        private int ImageNumber;

        public BingWallpaperManager()
        {
            InitiliseDirectories();


            var temp = new BingXMLParser().GetSpecificImageDetails(0);
            _imageList.Add(temp);
            DownloadImageManager.DownloadImage(temp);
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
                    _imageList.Add(details);
                    

                }
        }

        public void CacheThumbnails()
        {
            var cacheNumber = 0;
            for (; cacheNumber < 100; cacheNumber++)
            {
                var details = _defaultParser.GetSpecificImageDetails((uint)cacheNumber);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    DownloadImageManager.DownloadImage(details);
                    _thumbnails.Add(new Thumbnail(new BitmapImage(
                        new Uri(Directory.GetCurrentDirectory() + "\\thumbnails\\" + details.ImageFilePath))));
                });
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
                new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\img\\" + details.ImageFilePath));
        }

        public void NextWallpaper()
        {
            while (!(_imageList.Count >= ImageNumber + 2))
            {
            }

            ImageNumber++;
            UpdateWallpaper(_imageList[ImageNumber]);
        }

        public void PreviousWallpaper()
        {
            if (ImageNumber > 0)
            {
                ImageNumber--;
                UpdateWallpaper(_imageList[ImageNumber]);
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