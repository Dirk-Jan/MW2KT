using MW2KT_WPF.UI.TableGUI;
using MW2KTCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MW2KT_WPF
{
    /// <summary>
    /// Interaction logic for TableGUIWindow.xaml
    /// </summary>
    public partial class TableGUIWindow : Window
    {
        public TableGUIWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            cboxDevices.ItemsSource = PacketCapture.AvailableDevicesExtended;
            //lboxDevices.SelectedValue = new LivePacketDeviceExtended(PacketCapture.SelectedDevice);
            cboxDevices.SelectedIndex = PacketCapture.SelectedDeivceIndex;
            cboxDevices.SelectionChanged += CboxDevices_SelectionChanged;
        }

        private void CboxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PacketCapture.SelectedDevice = ((LivePacketDeviceExtended)e.AddedItems[0]).LivePacketDevice;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PacketCapture.PacketHandler.NewPlayerListAvailable += PacketHandler_NewPlayerListAvailable;
            new Thread(new ThreadStart(PacketCapture.StartCapturing)).Start();

            btnStartCapture.Content = "---";
        }

        private void PacketHandler_NewPlayerListAvailable(List<MW2KTCore.Packets.PckPartystatePlayer> players)
        {
            var newPlayers = new List<PlayerGridRow>();
            foreach (var p in players)
                newPlayers.Add(new PlayerGridRow(p));
            InvokeUpdatePlayerListInGUI(newPlayers);
        }

        private void InvokeUpdatePlayerListInGUI(List<PlayerGridRow> players)
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                UpdatePlayerListInGUI(players);
            });
        }

        private void UpdatePlayerListInGUI(List<PlayerGridRow> players)
        {
            dgGrid.ItemsSource = players;
        }

        private string[] GetRandomizedPartyImages()
        {
            string[] availableImages = { "party_red.png", "party_yellow.png", "party_orange.png", "party_pink.png", "party_blue.png", "party_bordeaux.png", "party_brown.png", "party_green.png", "party_purple.png" };
            return availableImages;
        }
    }
}
