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
        private static string mRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /*private static PacketHandler mPacketHandler = null;
        public static PacketHandler PacketHandler
        {
            get
            {
                if (mPacketHandler == null)
                    mPacketHandler = new PacketHandler();
                return mPacketHandler;
            }
            
        }*/

        private static LivePacketDeviceExtended mSelectedDevice = null;
        /// <summary>
        /// Returns the currently selected LivePacketDevice
        /// </summary>
        public static LivePacketDeviceExtended SelectedDevice
        {
            get
            {
                if (mSelectedDevice == null)         // Check if default deivce was loaded from file
                {
                    if(!LoadSelectedDeviceFromFile())    // Load default device from file.
                    {
                        if (AvailableDevicesExtended.Count > 0)
                        {
                            mSelectedDevice = AvailableDevicesExtended[0];
                        }
                    }
                }
                return mSelectedDevice;
            }
            set
            {
                mSelectedDevice = value;
                SaveSelectedDeviceToFile();
                Console.WriteLine("Saved currently selected device: " + mSelectedDevice.LivePacketDevice.Description);
            }
        }

        /// <summary>
        /// Returns the available LivePacketDevices on the local machine in an extended format.
        /// These can be passed into a control, the discription will be shown via the ToString() method.
        /// </summary>
        public static List<LivePacketDeviceExtended> AvailableDevicesExtended
        {
            get { return LivePacketDeviceExtended.AllLocalMachine; }
        }

        /// <summary>
        /// Loads the last selected LivePacketDevice from a file.
        /// </summary>
        private static bool LoadSelectedDeviceFromFile()
        {
            bool succes = false;
            try
            {
                using (StreamReader sr = new StreamReader(mRootPath + @"/device"))
                {
                    string name = sr.ReadLine();
                    foreach (var item in AvailableDevicesExtended)
                    {
                        if (item.LivePacketDevice.Name == name)
                        {
                            mSelectedDevice = item;
                            succes = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There was an error trying to load the settings for the capturedevice.", "MKTKT - ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return succes;
        }

        /// <summary>
        /// Save the last selected LivePacketDevice to a file.
        /// </summary>
        private static void SaveSelectedDeviceToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(mRootPath + @"/device", false))
                {
                    sw.WriteLine(SelectedDevice.LivePacketDevice.Name);
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show("There was a problem saving.");
            }
        }

        /// <summary>
        /// Starts capturing UPD packets related to Modern Warfare 2 and spits the caugth packages out to the PacketHandler.
        /// </summary>
        public static void StartCapturing(object o)
        {
            StartCapturing((PacketHandler)o);
        }
        public static void StartCapturing(PacketHandler packetHandler)
        {
            if(SelectedDevice == null)
            {
                MessageBox.Show("There are no capture devices available on the computer. Make sure WinPcap is installed.", "MW2KT - ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                using (PacketCommunicator communicator = SelectedDevice.LivePacketDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                {
                    communicator.SetFilter("ip and udp and port 28960");

                    // Start the capture
                    communicator.ReceivePackets(0, packetHandler.HandlePacket);
                }
            }
        }
    }
}
