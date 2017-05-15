using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2KTCore
{
    public static class PacketCapture
    {
        private static int mInterfaceIndex;

        /*private static void PreInintPcap()
        {
            // Init pcap
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            allDevices[0].
            foreach (LivePacketDevice lpd in allDevices)
                toolStripComboBox1.Items.Add(lpd.Description);

            if (File.Exists(Directory.GetCurrentDirectory() + @"\settings"))
            {
                byte[] settingsbuffer = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\settings");
                mInterfaceIndex = settingsbuffer[0];
            }
            if (allDevices.Count > 0)
                toolStripComboBox1.SelectedIndex = mInterfaceIndex;

            //toolStripComboBox1.SelectedIndexChanged += toolStripComboBox1_SelectedIndexChanged;
        }*/

        /*private static void InitPcap()
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
                communicator.ReceivePackets(0, PacketHandler.HandlePacket);
            }

        }*/

        public static IList<LivePacketDevice> Devices
        {
            get { return LivePacketDevice.AllLocalMachine; }
        }

        private static void StartCapturing(PacketDevice pd)
        {
            using (PacketCommunicator communicator = pd.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                communicator.SetFilter("ip and udp and port 28960");

                // Start the capture
                communicator.ReceivePackets(0, PacketHandler.HandlePacket);
            }
        }
    }
}
