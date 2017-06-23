using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bing_Wallpaper.Pages.Time_Line_Page
{
    /// <summary>
    /// Interaction logic for Time_Line_Page.xaml
    /// </summary>
    public partial class Time_Line_Page : UserControl, ISwitchable
    {
        public Time_Line_Page()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
