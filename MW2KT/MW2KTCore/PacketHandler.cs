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
            mLastReturnedPlayerList = players;
            if (NewPlayerListAvailable != null)
                NewPlayerListAvailable(players);
        }

        private List<PckPartystatePlayer> mLastReturnedPlayerList = new List<PckPartystatePlayer>();
        private List<PckPartystatePlayer> mTempPlayerList = null;

        public void HandlePacket(Packet packet)
        {
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
                Debug.WriteLine("Packettype: " + Encoding.UTF8.GetString(packetTypeBuffer));
                if (Encoding.UTF8.GetString(packetTypeBuffer).Contains("0partystate"))
                {
                    Debug.WriteLine("We have received a 0partystate packet");
                    // We have a 0partystate packet
                    PckPartystate partyPacket = new PckPartystate(mw2Payload);
                    // Check if packet was successfully read
                    if (!partyPacket.Success)
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
                        //if(!IsPlayerListTheSame(partyPacket.Players, mLastReturnedPlayerList))
                            OnNewPlayerListAvailable(partyPacket.Players);
                    }
                    else if (vali == "8") // We have the first part of all info
                    {
                        mTempPlayerList = partyPacket.Players;
                    }
                    else if (vali == "9") // We have the second part off all info
                    {
                        // We can also get the followup packet without getting a NEW first half packet first. The we use the old first half of the packet
                        if (mTempPlayerList != null) // Can only happen the first time
                        {
                            List<PckPartystatePlayer> players = mTempPlayerList;  // Add the players from the first half of the packet
                            foreach (PckPartystatePlayer p in partyPacket.Players)
                                if (!IsPlayerInList(p.SteamID, players))  // The last player in the first half of the packet and the first player in the second half of the packet are the same. So we filter the duplicate out.
                                    players.Add(p);
                            //RefreshPlayerList(players);
                            //mTempListPartyStatePlayers = null;
                            //if (!IsPlayerListTheSame(players, mLastReturnedPlayerList))
                                OnNewPlayerListAvailable(players);
                        }
                    }
                }
            }
        }

        private bool IsPlayerListTheSame(List<PckPartystatePlayer> list1, List<PckPartystatePlayer> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            for (int i = 0; i < list1.Count; i++)
            {
                PckPartystatePlayer p1 = list1[i];
                PckPartystatePlayer p2 = list2[i];
                if (p1.SteamID != p2.SteamID) return false;

            }
            return true;
        }

        private bool IsPlayerInList(UInt64 steamId, List<PckPartystatePlayer> list)
        {
            foreach (PckPartystatePlayer p in list)
                if (p.SteamID == steamId)
                    return true;
            return false;
        }


        #region Parties
        // Kijk naar party id
        // Staat hij nog niet in de list?
        // Zo nee, hebben meerdere spelers hetzelfde party id?
        // Zo ja, voeg party id toe aan list
        public List<UInt64> GetParties(List<PckPartystatePlayer> list)
        {
            var parties = new List<UInt64>();

            foreach(var p in list)
            {
                if (!IsUInt64InList(p.PartyID, parties) && MorePlayersInParty(p.SteamID, p.PartyID, list))
                    parties.Add(p.PartyID);
            }
            return parties;
        }

        private bool IsUInt64InList(UInt64 value, List<UInt64> list)
        {
            foreach (var i in list)
                if (i == value)
                    return true;
            return false;
        }

        private bool MorePlayersInParty(UInt64 ignoreSteamId, UInt64 partyId, List<PckPartystatePlayer> list)
        {
            foreach (var p in list)
                if (p.SteamID != ignoreSteamId && p.PartyID == partyId)
                    return true;
            return false;
        }
        #endregion
    }
}
