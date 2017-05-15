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

        public static IList<LivePacketDevice> Devices
        {
            get { return LivePacketDevice.AllLocalMachine; }
        }

        private static void LoadDefaultDevice()
        {

        }

        private static void SaveDefaultDevice()
        {

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
