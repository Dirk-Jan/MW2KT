using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MW2KTCore
{
    public static class PacketCapture
    {
        private static PacketHandler mPacketHandler = null;
        public static PacketHandler PacketHandler
        {
            get
            {
                if (mPacketHandler == null)
                    mPacketHandler = new PacketHandler();
                return mPacketHandler;
            }
            
        }

        private static string mRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
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
                Console.WriteLine("Currently selected device: " + mDefaultDevice.Description);
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
            //MessageBox.Show(mRootPath + @"/device");
            using (StreamWriter sw = new StreamWriter(mRootPath + @"/device", false))
            {
                sw.WriteLine(DefaultDevice.Name);
            }
        }

        public static void StartCapturing(/*HandlePacket packetHandler*/)
        {
            using (PacketCommunicator communicator = DefaultDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                communicator.SetFilter("ip and udp and port 28960");

                // Start the capture
                communicator.ReceivePackets(0, PacketHandler.HandlePacket);
            }
        }
    }
}
