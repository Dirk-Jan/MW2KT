using MW2KTCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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
        protected PacketHandler mPacketHandler = new PacketHandler();

        public TableGUIWindow()
        {
            InitializeComponent();

            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(dgGrid.Items);
            ((INotifyCollectionChanged)myCollectionView).CollectionChanged += new NotifyCollectionChangedEventHandler(DataGrid_CollectionChanged);
        }

        private void DataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            /*foreach (Player item in dgGrid.ItemsSource)
            {
                if (item.IsHost)
                {
                    var row = dgGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    row.Background = Brushes.Beige;
                }
            }*/
        }

        protected virtual void Window_ContentRendered(object sender, EventArgs e)
        {
            cboxDevices.ItemsSource = PacketCapture.AvailableDevicesExtended;
            //lboxDevices.SelectedValue = new LivePacketDeviceExtended(PacketCapture.SelectedDevice);
            cboxDevices.SelectedIndex = PacketCapture.SelectedDevice.Index;
            cboxDevices.SelectionChanged += CboxDevices_SelectionChanged;
        }

        private void CboxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PacketCapture.SelectedDevice = (LivePacketDeviceExtended)e.AddedItems[0];
        }

        protected virtual void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mPacketHandler.NewPlayerListAvailable += PacketHandler_NewPlayerListAvailable;

            var t = new Thread(new ParameterizedThreadStart(PacketCapture.StartCapturing));
            t.Start(mPacketHandler);

            btnStartCapture.Content = "---";
        }

        protected void PacketHandler_NewPlayerListAvailable(List<MW2KTCore.Packets.PckPartystatePlayer> players)
        {
            var newPlayers = new List<Player>();
            foreach (var p in players)
                newPlayers.Add(new Player(p));
            InvokeUpdatePlayerListInGUI(newPlayers);
        }

        private void InvokeUpdatePlayerListInGUI(List<Player> players)
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                UpdatePlayerListInGUI(players);
            });
        }

        private void UpdatePlayerListInGUI(List<Player> players)
        {
            dgGrid.ItemsSource = players;
            /*int iPlayer = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if(players[i].IsHost)
                {
                    iPlayer = i;
                    break;
                }
            }
            dgGrid.RowBackground = Brushes.Beige;*/
            
        }

        private string[] GetRandomizedPartyImages()
        {
            string[] availableImages = { "party_red.png", "party_yellow.png", "party_orange.png", "party_pink.png", "party_blue.png", "party_bordeaux.png", "party_brown.png", "party_green.png", "party_purple.png" };
            return availableImages;
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (dgGrid.SelectedIndex >= 0)
            {
                var player = (Player)dgGrid.SelectedItem;
                //MessageBox.Show(player.Name);
                //MessageBox.Show(player.Tag.ToString());
                if (player.Tag == PlayerTag.Cheater)
                {
                    //MessageBox.Show("Cheater");
                    player.Tag = PlayerTag.None;
                    player.SavePlayerToFile();
                }
                else
                {
                    //MessageBox.Show("None");
                    player.Tag = PlayerTag.Cheater;
                    player.SavePlayerToFile();
                }
                dgGrid.Items.Refresh();
            }
        }

        private void TextBlock_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (dgGrid.SelectedIndex >= 0)
            {
                /*var player = (Player)dgGrid.SelectedItem;
                var dir = @"H:\MW2KT_packets\match1\";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllBytes(dir + dgGrid.SelectedIndex.ToString() + " " + player.Name, player.PlayerBuffer);*/
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Player item in dgGrid.ItemsSource)
            {
                if (item.IsHost)
                {
                    var row = dgGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    row.Background = Brushes.Beige;
                }
            }
        }
    }
}
