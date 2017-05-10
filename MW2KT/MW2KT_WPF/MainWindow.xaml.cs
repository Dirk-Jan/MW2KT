using MW2KTCore;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MW2KT_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UI.PlayerView mLabel;
        public MainWindow()
        {
            InitializeComponent();

            mLabel = new UI.PlayerView("^1G^7hostPhRea^0K")
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            //grdMain.Children.Add(mLabel);
            tboxInput.TextChanged += TboxInput_TextChanged;

            //imgTest.Source = new BitmapImage(new Uri("http://cdn.edgecast.steamstatic.com/steamcommunity/public/images/avatars/48/48e0836bb433ebc476b3a06519f458e2cec45dfc_full.jpg"));
        }

        private void TboxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //mLabel.Text = tboxInput.Text;
            //lblMW2.Text = tboxInput.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //tboxInput.Text = Steam.GetPlayerAvatar(76561198092449962);
            mw2PlayerView1.SteamID = 76561198092449962;
            //mw2PlayerView1.SteamID = 76561198128508608;
            mw2PlayerView1.PlayerName = mw2PlayerView1.PlayerName;
        }
    }
}
