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
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //tboxInput.Text = Steam.GetPlayerAvatar(76561198092449962);
            //mw2PlayerView1.SteamID = 76561198092449962;
            //mw2PlayerView1.SteamID = 76561198128508608;
            //mw2PlayerView1.PlayerName = mw2PlayerView1.PlayerName;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 18; i++)
            {
                UI.PlayerViewLive p = new UI.PlayerViewLive();
                p.PlayerName = "123456789012";
                //p.SteamID = 76561198092449962;
                if (i < 9)
                {
                    Grid.SetRow(p, i);
                }
                else
                {
                    Grid.SetColumn(p, 1);
                    Grid.SetRow(p, i - 9);
                }
                grdPlayerViews.Children.Add(p);
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            lboxDevices.Items.Clear();
            foreach (var item in PacketCapture.Devices)
            {
                lboxDevices.Items.Add(item.Description);
            }
            //lboxDevices.ItemsSource = PacketCapture.Devices;
        }

        private void lboxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*string s = e.AddedItems[0].ToString();
            foreach (var item in PacketCapture.Devices)
            {
                if (item.Name == name)
                    PacketCapture.DefaultDevice = item;
            }
            MessageBox.Show(s);*/
        }
    }
}
