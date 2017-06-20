using MW2KTCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        PacketHandler mPacketHandler = new PacketHandler();
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
                UI.ScoreboardGUI.PlayerViewLive p = new UI.ScoreboardGUI.PlayerViewLive();
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
            /*lboxDevices.Items.Clear();
            foreach (var item in PacketCapture.AvailableDevices)
            {
                lboxDevices.Items.Add(item.Description);
            }
            //lboxDevices.ItemsSource = PacketCapture.Devices;*/
        }

        private void lboxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*string description = e.AddedItems[0].ToString();
            foreach (var item in PacketCapture.AvailableDevices)
            {
                Console.WriteLine(description + " : " + item.Description);
                if (item.Description == description)
                    PacketCapture.SelectedDevice = item;
            }
            //MessageBox.Show(name);
            //System.Windows.Forms.MessageBox.Show(e.AddedItems.Count.ToString());
            //e.AddedItems[0]*/
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //PacketCapture.PacketHandler.NewPlayerListAvailable += PacketHandler_NewPlayerListAvailable;
            //new Thread(new ThreadStart(PacketCapture.StartCapturing)).Start();

            
            //mPacketHandler.NewPlayerListAvailable += PacketHandler_NewPlayerListAvailable;

            var t = new Thread(new ParameterizedThreadStart(PacketCapture.StartCapturing));
            t.Start(mPacketHandler);

            //PacketCapture.StartCapturing();
            btnStartCapture.Content = "---";
        }

        private void PacketHandler_NewPlayerListAvailable(List<MW2KTCore.Packets.PckPartystatePlayer> players)
        {
            InvokeUpdatePlayerListInGUI(players);
        }

        private void InvokeUpdatePlayerListInGUI(List<MW2KTCore.Packets.PckPartystatePlayer> players)
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                UpdatePlayerListInGUI(players);
            });
        }

        private void UpdatePlayerListInGUI(List<MW2KTCore.Packets.PckPartystatePlayer> players)
        {
            var parties = mPacketHandler.GetParties(players);
            var partyImages = GetRandomizedPartyImages();
            for (int i = 0; i < players.Count; i++)
            {
                var p = new UI.ScoreboardGUI.PlayerViewLive();                                    // Create new PlayerView
                p.LoadPlayerInfoInView(players[i]);                 // Load generic info in view
                for (int j = 0; j < parties.Count; j++)             // Set the party icon if in party
                {
                    if (parties[j] == players[i].PartyID)
                        p.SetPartyImage(partyImages[j]);
                }


                if (i < 9)                          // Set the location in the grid
                {
                    Grid.SetRow(p, i);
                }
                else
                {
                    Grid.SetColumn(p, 1);
                    Grid.SetRow(p, i - 9);
                }
                grdPlayerViews.Children.Add(p);     // Add PlayerView to grid
            }
        }

        private string[] GetRandomizedPartyImages()
        {
            string[] availableImages = { "party_red.png", "party_yellow.png", "party_orange.png", "party_pink.png", "party_blue.png", "party_bordeaux.png", "party_brown.png", "party_green.png", "party_purple.png" };
            return availableImages;
        }
    }
}
