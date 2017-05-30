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
        private static LivePacketDevice mSelectedDevice = null;
        public static LivePacketDevice SelectedDevice
        {
            get
            {
                if (mSelectedDevice == null)         // Check if default deivce was loaded from file
                    LoadSelectedDeviceFromFile();    // Load default device from file.
                return mSelectedDevice;
            }
            set
            {
                mSelectedDevice = value;
                Console.WriteLine("Currently selected device: " + mSelectedDevice.Description);
                SaveSelectedDeviceToFile();
            }
        }

        public static int SelectedDeivceIndex
        {
            get
            {
                for (int i = 0; i < AvailableDevices.Count; i++)
                    if (AvailableDevices[i].Name == SelectedDevice.Name)
                        return i;
                return -1;
            }
        }
        public static IList<LivePacketDevice> AvailableDevices
        {
            get { return LivePacketDevice.AllLocalMachine; }
        }

        public static List<LivePacketDeviceExtended> AvailableDevicesExtended
        {
            get
            {
                var list = new List<LivePacketDeviceExtended>();
                foreach (var item in LivePacketDevice.AllLocalMachine)
                    list.Add(new LivePacketDeviceExtended(item));
                return list;
            }
        }

        private static void LoadSelectedDeviceFromFile()
        {
            using (StreamReader sr = new StreamReader(mRootPath + @"/device"))
            {
                string name = sr.ReadLine();
                foreach (var item in AvailableDevices)
                {
                    if (item.Name == name)
                        mSelectedDevice = item;
                }
                if(mSelectedDevice == null)
                {
                    MessageBox.Show("Could not locate your default capture device on this computer.", "MW2KT", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private static void SaveSelectedDeviceToFile()
        {
            //MessageBox.Show(mRootPath + @"/device");
            using (StreamWriter sw = new StreamWriter(mRootPath + @"/device", false))
            {
                sw.WriteLine(SelectedDevice.Name);
            }
        }

        public static void StartCapturing()
        {
            using (PacketCommunicator communicator = SelectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                communicator.SetFilter("ip and udp and port 28960");

                // Start the capture
                communicator.ReceivePackets(0, PacketHandler.HandlePacket);
            }
        }
    }
}
