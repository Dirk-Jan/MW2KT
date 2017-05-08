using MW2KT.Packets;
using MW2KT.UI;
using PcapDotNet.Core;
using PcapDotNet.Core.Extensions;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MW2KT
{
    public partial class Main : Form
    {
        private List<Point> mPlayerViewPoints = new List<Point>();
        //private List<PlayerOLD> mPlayers = new List<PlayerOLD>();
        private string mLogPath, mLogfileSavePath = Directory.GetCurrentDirectory() + @"\logpath.txt";
        private byte mInterfaceIndex = 0;

        public Main()
        {
            InitializeComponent();
            PlayerView p = new PlayerView();
            ClientSize = new Size((p.Margin.Left + p.Width + p.Margin.Right) * 2, menuStrip1.Height + p.Margin.Top + (p.Height + p.Margin.Bottom) * 9);

            // Calculate points
            // Left column
            for (int i = 0; i < 9; i++)
            {
                int initialOffset = menuStrip1.Height + p.Margin.Top;
                int playerControls = ((p.Size.Height + p.Margin.Top) * i);
                int seperationSpace = 0;
                if (i > 2)   // So the fourth one
                    seperationSpace = p.Margin.Top * 2;
                if (i > 5)
                    seperationSpace = p.Margin.Top * 4;
                mPlayerViewPoints.Add(new Point(p.Margin.Left, initialOffset + playerControls + seperationSpace));
                //mPlayerViewPoints.Add(new Point(p.Margin.Left, menuStrip1.Height + p.Margin.Top + ((p.Size.Height + p.Margin.Top) * i)));
            }
            // Right column
            for (int i = 0; i < 9; i++)
            {
                int initialOffset = menuStrip1.Height + p.Margin.Top;
                int playerControls = ((p.Size.Height + p.Margin.Top) * i);
                int seperationSpace = 0;
                if (i > 2)   // So the fourth one
                    seperationSpace = p.Margin.Top * 2;
                if (i > 5)
                    seperationSpace = p.Margin.Top * 4;
                mPlayerViewPoints.Add(new Point(p.Margin.Left + p.Width + p.Margin.Left + p.Margin.Right, initialOffset + playerControls + seperationSpace));
                //mPlayerViewPoints.Add(new Point(p.Margin.Left + p.Width + p.Margin.Left + p.Margin.Right, menuStrip1.Height + p.Margin.Top + ((p.Size.Height + p.Margin.Top) * i)));
            }

            ClientSize = new Size((p.Margin.Left + p.Width + p.Margin.Right) * 2, mPlayerViewPoints[8].Y + p.Size.Height + p.Margin.Bottom);

            // Load logpath
            if (File.Exists(mLogfileSavePath))
            {
                using (StreamReader sr = new StreamReader(mLogfileSavePath))
                    mLogPath = sr.ReadLine();
                //if(!String.IsNullOrEmpty(mLogPath))
                //Start file mining
            }
            
            // Init pcap
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            foreach (LivePacketDevice lpd in allDevices)
                toolStripComboBox1.Items.Add(lpd.Description);

            if (File.Exists(Directory.GetCurrentDirectory() + @"\settings"))
            {
                byte[] settingsbuffer = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\settings");
                mInterfaceIndex = settingsbuffer[0];
            }
            if (allDevices.Count > 0)
                toolStripComboBox1.SelectedIndex = mInterfaceIndex;

            toolStripComboBox1.SelectedIndexChanged += toolStripComboBox1_SelectedIndexChanged;
        }

        #region Buttons
        private void setMW2PathToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showBanListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 18; i++)
            {
                PlayerView p = new PlayerView();
                p.SetName("I'm a player");
                p.SetRank(4, 52);
                p.SetIPAddress("10.1.10.20");
                p.Location = mPlayerViewPoints[i];
                Controls.Add(p);
            }
        }
        #endregion

        private void Main_Shown(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(InitPcap)).Start();
        }

        #region EditControlCrossThread
        private void AddPlayerView_ct(PlayerView p)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    AddPlayerView_ct(p);
                });
            }
            else this.Controls.Add(p);
        }
        private void ClearPlayerList_ct()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    ClearPlayerList_ct();
                });
            }
            else
            {
                for (int i = 0; i < Controls.Count; i++)
                {
                    if (Controls[i] is PlayerView)
                    {
                        Controls.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        #endregion

        #region PCAP
        private void InitPcap()
        {
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;


            if (allDevices.Count == 0)
            {
                MessageBox.Show("No interfaces found! Make sure WinPcap is installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Take the selected adapter
            PacketDevice selectedDevice = allDevices[mInterfaceIndex];

            // Open the device
            using (PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                communicator.SetFilter("ip and udp and port 28960");

                // Start the capture
                communicator.ReceivePackets(0, PacketHandler);
            }

        }

        private List<PckPartystatePlayer> mTempListPartyStatePlayers = null;
        private void PacketHandler(Packet packet)
        {
            //IpV4Datagram ip = packet.Ethernet.IpV4;
            //UdpDatagram udp = ip.Udp;
            // First 14 pcap Header, next 20 IPV4 Header, next 8 UDP Header
            // Get the MW2 Payload
            byte[] mw2Payload = new byte[packet.Length - 42];
            Buffer.BlockCopy(packet.Buffer, 42, mw2Payload, 0, mw2Payload.Length);

            // Check if packettype is 0partystate
            if (mw2Payload.Length > 15)
            {
                // Check if packettype is 0partystate
                Byte[] packetTypeBuffer = new Byte[16];
                Buffer.BlockCopy(mw2Payload, 0, packetTypeBuffer, 0, packetTypeBuffer.Length);
                //Debug.WriteLine("Packettype: " + Encoding.UTF8.GetString(temp));
                if (Encoding.UTF8.GetString(packetTypeBuffer).Contains("0partystate"))
                {
                    Debug.WriteLine("We have received a 0partystate packet");
                    // We have a 0partystate packet
                    PckPartystate partyPacket = new PckPartystate(mw2Payload);
                    // Check if packet was successfully read
                    if (!partyPacket.mSuccess)
                        return;
                    Debug.WriteLine("We've got a successfully read partystate packet with packettype: " + partyPacket.PacketType.ToString("X2"));
                    // Check if multipart packet or not
                    /*using (StreamWriter sw = new StreamWriter(@"H:/mw2_received_packettypes.txt", true))
                    {
                        sw.WriteLine(partyPacket.PacketType.ToString("X2"));
                    }*/
                    string vali = partyPacket.PacketType.ToString("X2").Substring(1, 1);
                    if (vali == "4") // we have all info
                    {
                        RefreshPlayerList(partyPacket.Players);
                    }
                    else if (vali == "8") // We have the first part of all info
                    {
                        mTempListPartyStatePlayers = partyPacket.Players;
                    }
                    else if (vali == "9") // We have the second part off all info
                    {
                        // We can also get the followup packet without getting a NEW first half packet first. The we use the old first half of the packet
                        if (mTempListPartyStatePlayers != null) // Can only happen the first time
                        {
                            List<PckPartystatePlayer> players = mTempListPartyStatePlayers;  // Add the players from the first half of the packet
                            foreach (PckPartystatePlayer p in partyPacket.Players)
                                if (!IsPlayerInList(p.SteamID, players))  // The last player in the first half of the packet and the first player in the second half of the packet are the same. So we filter the duplicate out.
                                    players.Add(p);
                            RefreshPlayerList(players);
                            //mTempListPartyStatePlayers = null;
                        }
                    }
                }
            }
        }

        private bool IsPlayerInList(UInt64 steamId, List<PckPartystatePlayer> list)
        {
            foreach (PckPartystatePlayer p in list)
                if (p.SteamID == steamId)
                    return true;
            return false;
        }

        private bool IsUint64InList(UInt64 value, List<UInt64> list)
        {
            foreach (UInt64 i in list)
                if (i == value)
                    return true;
            return false;
        }

        private bool MorePlayersInParty(UInt64 ignoreSteamId, UInt64 partyId, List<PckPartystatePlayer> list)
        {
            foreach (PckPartystatePlayer p in list)
                if (p.SteamID != ignoreSteamId && p.PartyID == partyId)
                    return true;
            return false;
        }
        #endregion

        List<PckPartystatePlayer> mOldPlayerList = new List<PckPartystatePlayer>();
        private bool IsPlayerListTheSame(List<PckPartystatePlayer> players, List<PckPartystatePlayer> players2)
        {
            if (players.Count != players2.Count)
                return false;
            foreach (var player in players)
                if (!IsPlayerInList(player.SteamID, players2))
                    return false;
            return true;
        }
        /*private bool IsPlayerInList(UInt64 steamID, List<PckPartystatePlayer> list)
        {
            foreach (var player in list)
            {
                if (player.SteamID == steamID)
                    return true;
            }
            return false;
        }*/

        private void RefreshPlayerList(List<PckPartystatePlayer> players)
        {
            /*if (IsPlayerListTheSame(players, mOldPlayerList))
                return;
            mOldPlayerList = players;*/

            List<PlayerOLD> playerList = new List<PlayerOLD>();
            foreach (PckPartystatePlayer p in players)
            {
                playerList.Add(new PlayerOLD(p.SteamID, p.PlayerName, p.PartyID, p.ExternalIP, p.PlayerLevel, p.PlayerPrestigeLevel));
            }
            ClearPlayerList_ct();

            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].View.SetViewLocation(mPlayerViewPoints[i]);
                AddPlayerView_ct(playerList[i].View);
            }

            List<UInt64> parties = new List<UInt64>();
            foreach (PckPartystatePlayer p in players)
                if (!IsUint64InList(p.PartyID, parties) && MorePlayersInParty(p.SteamID, p.PartyID, players))
                    parties.Add(p.PartyID);
            // Now we've got all parties
            for (int i = 0; i < parties.Count; i++)
                foreach (PlayerOLD p in playerList)
                    if (p.PartyID == parties[i])
                        p.SetPartyImage(GetPartyImage(i));
        }

        private Image GetPartyImage(int index)  // Just a simple, quick method to make sure all parties have a different color
        {
            switch (index)
            {
                case 0:
                    return MW2KT.Properties.Resources.party_red;
                case 1:
                    return MW2KT.Properties.Resources.party_yellow;
                case 2:
                    return MW2KT.Properties.Resources.party_green;
                case 3:
                    return MW2KT.Properties.Resources.party_pink;
                case 4:
                    return MW2KT.Properties.Resources.party_orange;
                case 5:
                    return MW2KT.Properties.Resources.party_blue;
                case 6:
                    return MW2KT.Properties.Resources.party_purple;
                case 7:
                    return MW2KT.Properties.Resources.party_brown;
                case 8:
                    return MW2KT.Properties.Resources.party_bordeaux;
                default:
                    return null;
            }
        }

        /*private PlayerOLD GetPlayerBySteamID(UInt64 id)
        {
            foreach (PlayerOLD p in mPlayers)
                if (p.SteamID == id)
                    return p;
            return null;
        }*/

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] a = new byte[1];
            a[0] = (byte)toolStripComboBox1.SelectedIndex;
            File.WriteAllBytes(Directory.GetCurrentDirectory() + @"\settings", a);
        }
    }
}
