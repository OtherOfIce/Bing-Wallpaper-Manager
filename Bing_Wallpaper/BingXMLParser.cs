using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bing_Wallpaper
{
    class BingXMLParser : IParser
    {

        public ImageDetails GetNextImageDetails()
        {
            throw new NotImplementedException();
        }

        public ImageDetails GetPreviousImageDetails()
        {
            throw new NotImplementedException();
        }

        public ImageDetails GetSpecificImageDetails(uint imgNum)
        {
            XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
            xmlDoc.Load("https://www.bing.com/HPImageArchive.aspx?format=xml&idx=" + imgNum + "&n=1&mkt=en-us");
            XmlNodeList test = xmlDoc.GetElementsByTagName("urlBase");
            ImageDetails details;
            details.ImageUri = new Uri(@"https://www.bing.com" + test[0].InnerText + "_1920x1080.jpg");
            details.ImageFilePath = test[0].InnerText.Split('/').Last().Split('_').First() + ".jpg";
            return details;
        }

        public uint GetImageNumber()
        {
            throw new NotImplementedException();
        }

        public void SetImageNumber()
        {
            throw new NotImplementedException();
        }
    }
}
