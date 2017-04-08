using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bing_Wallpaper
{
    class BingXMLParser
    {
        public static String getImageURL(uint imageNumber)
        {
            XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
            xmlDoc.Load("https://www.bing.com/HPImageArchive.aspx?format=xml&idx=" + imageNumber + "&n=1&mkt=en-us");
            XmlNodeList test = xmlDoc.GetElementsByTagName("urlBase");
            return @"https://www.bing.com" + test[0].InnerText + "_1920x1080.jpg";
        }
    }
}
