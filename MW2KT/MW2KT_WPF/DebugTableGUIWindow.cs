using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MW2KT_WPF
{
    public class DebugTableGUIWindow : TableGUIWindow
    {
        private int count = 0;
        private string[] packets;
        public DebugTableGUIWindow()
        {
            btnStartCapture.Content = "Read Next Packet";
            cboxDevices.IsEnabled = false;
            mPacketHandler.NewPlayerListAvailable += PacketHandler_NewPlayerListAvailable;

            //packets = Directory.GetFiles(@"H:\MW2KT_packets\match2\");
            packets = Directory.GetFiles(@"C:\Users\Dirk-Jan de Beijer\Documents\12ois\MW2KT_packets\match6\");
        }

        protected override void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(count < packets.Length)
            {
                mPacketHandler.HandlePacket(new Packet(File.ReadAllBytes(packets[count]), DateTime.Now, DataLinkKind.IpV4));
                count++;
                this.Title = count + "/" + packets.Length + " packets read";
            }
        }

        protected override void Window_ContentRendered(object sender, EventArgs e)
        {
            // Do nothing
        }
    }
}
