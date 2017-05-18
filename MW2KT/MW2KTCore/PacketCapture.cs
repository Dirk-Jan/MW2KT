using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2KTCore
{
    public static class PacketCapture
    {
        private static string mRootPath = "";
        private static LivePacketDevice mDefaultDevice = null;
        public static LivePacketDevice DefaultDevice
        {
            get
            {
                if (mDefaultDevice == null)         // Check if default deivce was loaded from file
                    LoadDefaultDeviceFromFile();    // Load default device from file.
                return mDefaultDevice;
            }
            set
            {
                mDefaultDevice = value;
                SaveDefaultDeviceToFile();
            }
        }
        public static IList<LivePacketDevice> Devices
        {
            get { return LivePacketDevice.AllLocalMachine; }
        }

        private static void LoadDefaultDeviceFromFile()
        {
            using (StreamReader sr = new StreamReader(mRootPath + @"/device"))
            {
                string name = sr.ReadLine();
                foreach (var item in Devices)
                {
                    if (item.Name == name)
                        mDefaultDevice = item;
                }
            }
        }

        private static void SaveDefaultDeviceToFile()
        {
            using (StreamWriter sw = new StreamWriter(mRootPath + @"/device", false))
            {
                sw.WriteLine(DefaultDevice.Name);
            }
        }

        private static void StartCapturing(HandlePacket packetHandler)
        {
            using (PacketCommunicator communicator = DefaultDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                communicator.SetFilter("ip and udp and port 28960");

                // Start the capture
                communicator.ReceivePackets(0, packetHandler);
            }
        }
    }
}
