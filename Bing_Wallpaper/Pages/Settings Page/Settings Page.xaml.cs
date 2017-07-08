using System;
using System.Windows.Controls;

namespace Bing_Wallpaper
{
    
    /// <summary>
    /// Interaction logic for Settings_Page.xaml
    /// </summary>
    public partial class Settings_Page : UserControl, ISwitchable
    {

        public Settings_Page()
        {
            
            InitializeComponent();
            
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
