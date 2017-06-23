using System;

namespace Bing_Wallpaper
{
    public struct ImageDetails
    {
        public String ImageFilePath;
        public Uri ImageUri;
    }
    interface IParser
    {
        ImageDetails GetNextImageDetails();
        ImageDetails GetPreviousImageDetails();
        ImageDetails GetSpecificImageDetails(uint imageNumber);
        uint GetImageNumber();
        void SetImageNumber();
    }
}
