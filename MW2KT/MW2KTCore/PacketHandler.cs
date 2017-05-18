using MW2KTCore.Packets;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2KTCore     // Only reason this class is not static is because of the issue with the GC with a static event.
{
    public class PacketHandler
    {
        public delegate void NewPlayerListAvailableEventHandler(List<PckPartystatePlayer> players);
        public event NewPlayerListAvailableEventHandler NewPlayerListAvailable;

        protected virtual void OnNewPlayerListAvailable(List<PckPartystatePlayer> players)
        {
            if (NewPlayerListAvailable != null)
                NewPlayerListAvailable(players);
        }

        private List<PckPartystatePlayer> mTempListPartyStatePlayers = null;

        //public delegate void NewPlayerListAvailable(List<PckPartystatePlayer> players);

        public void HandlePacket(Packet packet)
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
                        //RefreshPlayerList(partyPacket.Players);
                        OnNewPlayerListAvailable(partyPacket.Players);
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
                            //RefreshPlayerList(players);
                            //mTempListPartyStatePlayers = null;
                            OnNewPlayerListAvailable(players);
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
    }
}
