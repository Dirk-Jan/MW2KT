using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MW2KT_WPF.UI
{
    public class PlayerView : UserControl
    {
        public PlayerView(string name)
        {
            this.Background = Brushes.Gray;
            Grid grdMain = new Grid();
            // Add stuff to player view
            Image img = new Image();
            img.MinWidth = 32;
            img.MinHeight = 32;
            img.MaxWidth = 64;
            img.MaxHeight = 64;
            img.Source = new BitmapImage(new Uri("http://cdn.edgecast.steamstatic.com/steamcommunity/public/images/avatars/48/48e0836bb433ebc476b3a06519f458e2cec45dfc_full.jpg"));
            grdMain.Children.Add(img);

            grdMain.Children.Add(new MW2Label() { Text = name });
            this.AddChild(grdMain);
        }
    }
}
