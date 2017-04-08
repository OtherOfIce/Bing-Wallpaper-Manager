using System;
using System.Runtime.InteropServices;


namespace Change_Wallpaper
{
    class ChangeWallpaper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(int uiAction, uint uiParam, String pvParam, int fWinIni);

        public static void SetBackground(string wallpaperPath)
        {
            int None = 0x00;
            int SPIF_UPDATEINIFILE = 0x01;
            int SPIF_SENDWININICHANGE = 0x02;
            int SPI_SETDESKWALLPAPER = 0x0014;
            for (int i = 0; i < 5; i++)
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, wallpaperPath,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }
    }
}