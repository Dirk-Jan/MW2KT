using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MW2KTCore.Packets
{
    public class PckPartystate
    {
        // 0partystate
        // First 14 bytes are pcap related
        // Next 20 are IPv4 Header, if !(IHL > 5)
        // Next 8 are UDP Header
        // Rest is MW2 packet (> 42)

        // FF FF FF FF 30 70 61 72 74 79 73 74 61 74 65 00 (16 bytes)
        // FF FF FF FF 0partystate
        // Partystate header starts after 0partystate
        public Byte[] PacketID { get; set; }        // 4 bytes      // byte 1 unknown
                                                    // byte 2 seems to increment by 2 with every new packet set
                                                    // byte 3 unknown
                                                    // byte 4 always 0x00
        public Byte PacketType { get; set; }        // 1 byte (5th byte 0xX9 --> header short, players after) if 39 packet only contains 2 more bytes 
        // If 0xX8, 0xX9 will follow with the other players. If 0xX4 you have all the info in one packet.
        public Byte PlayerCount { get; set; }       // 1 byte
                                                    // byte[8] always 0                         // 8 bytes
                                                    // byte always 2E                           // 1 byte
                                                    // byte[3] always 0                         // 3 bytes
                                                    // byte is most of the time the same in a game, but can change
                                                    // 1 byte
                                                    // byte[2] mostly 0x00 0x00 but not when the previous byte changed
                                                    // 2 bytes
                                                    // byte[2] always 0                         // 2 bytes
                                                    // byte[2] mostly 0, but have seen it be something else (H:\deleteme_packets\test 7 online\4 length=184)
                                                    // 2 bytes
                                                    // byte[2] always 0                         // 2 bytes
        public Byte MaxPlayerCount { get; set; }    // 1 byte       (not sure)
        // byte always 0x00                         // 1 byte
        // byte always < 0x0A                       // 1 byte       (free playerslots?)
        public Byte ValidationByte { get; set; }    // 1 byte
        public UInt64 LobbyID { get; set; }         // 8 bytes      (not sure, but looks like party id)
        public IPAddress HostIP_int { get; set; }   // 4 bytes       // Host's internal or external IP
        public IPAddress HostIP_ext { get; set; }   // 4 bytes       // Host's external IP
        public UInt16 HostPort_int { get; set; }    // 2 bytes
        public UInt16 HostPort_ext { get; set; }    // 2 bytes
                                                    // byte[40] always 0x00                     // 40 bytes
                                                    // byte[3] always the same in a single game // 3 bytes
                                                    // byte[5] always B1 00 00 86 01 AFAIK      // 5 bytes
                                                    // byte always 00 except private game AFAIK // 1 byte



        // byte[4] always FF unless private game    // 4 bytes
        // byte[6] mostly the same per game but can change aswell (H:\deleteme_packets\test 6 online (modded lobby))
        // 6 bytes
        // byte always 0x02 AFAIK                   // 1 byte
        // byte[3] always 0x00 AFAIK                // 3 bytes
        //#ENDOFHEADER
        // ignore below for now, there are always 14 bytes left, sometimes 6 are FF sometimes only 5 or 4 are

        // Some things are not for sure eventhough there is no "AFAIK"

        // The validation byte is the only thing that changes when 00 01 02 03 04 player occurs
        // Occurs when validation byte is not 0xX4 or 0xXC



        //OLD
        // byte[6] either all 0x00 or 0xFF          // 6 bytes      // (AFAIK 0x00 private, lobby, friends and 0xFF online)
        // if 0xFF 12 bytes before first playername
        // Not always it seems
        // FF FF FF FF 02 04 00 01 02 00 00 00 00 01 02 03 04 05 06 07 EA DD 03 57 68 -A packet at the beginning of a game
        // 07 is the first players id, playername starts with the 57
        // else if 0x00 1 byte before first playername

        // if 00 00 00 player will follow

        // if byte before playercount == 0x19 || 0x29 || 0x39 || 0x69 --> header ends player starts
        //OLD

        //private int mMW2PayloadOffset, playerIndex = 0;
        private bool mSucces = true;
        public bool Success
        {
            get { return mSucces; }
            set { mSucces = value; }
        }
        public List<PckPartystatePlayer> Players { get; set; }

        // Constructor
        public PckPartystate(byte[] buffer) // The buffer needs to be from the beginning off FF FF FF FF partystate, so the udp payload
        {
            int mMW2PayloadOffset, playerIndex = 0;

            PacketID = new Byte[4];
            Buffer.BlockCopy(buffer, 16, PacketID, 0, PacketID.Length);
            PacketType = buffer[20];
            PlayerCount = buffer[21];
            if (PacketType == 0x39) // Only 2 bytes follow, useless packet
            {
                Success = false;
                return;
            }
            Players = new List<PckPartystatePlayer>();
            if (PacketType.ToString("X2").Substring(1, 1) == "9") // short package / follow up package
            {
                mMW2PayloadOffset = 22;
                // Extract players
                // A followup packet will start with 8 bytes as the last 8 bytes, then 1x 7 bytes, 3x 8 bytes, 1x 7 bytes, 3x 8 bytes, ect
                while ((buffer.Length - mMW2PayloadOffset) >= 73) // 73 is the minimum amount of bytes needed for a packet without a playername
                {
                    Byte[] temp = new Byte[buffer.Length - mMW2PayloadOffset];
                    Buffer.BlockCopy(buffer, mMW2PayloadOffset, temp, 0, temp.Length);
                    // 1, 5, 9, 13, ect shortend
                    // 0, 4, 8, 12, ect %4 0 so index==1 || (((index-1)%4)==0) > shortend
                    bool shortend;
                    if (playerIndex == 1 || (((playerIndex - 1) % 4) == 0))
                        shortend = true;
                    else
                        shortend = false;
                    PckPartystatePlayer p = new PckPartystatePlayer(temp, true, shortend, HostIP_ext);  // HostIP_ext is not set here, maybe the host is always in the first package
                    Players.Add(p);

                    // increment playerindex and mw2payloadoffset
                    playerIndex++;
                    mMW2PayloadOffset += p.PlayerPayloadLength;
                }
            }
            else
            {
                MaxPlayerCount = buffer[43];
                ValidationByte = buffer[46];
                LobbyID = BitConverter.ToUInt64(buffer, 47);
                Byte[] temp = new Byte[4];
                Buffer.BlockCopy(buffer, 55, temp, 0, 4);
                HostIP_int = new IPAddress(temp);
                Buffer.BlockCopy(buffer, 59, temp, 0, 4);
                HostIP_ext = new IPAddress(temp);
                HostPort_int = BitConverter.ToUInt16(buffer, 63);
                HostPort_ext = BitConverter.ToUInt16(buffer, 65);
                mMW2PayloadOffset = 130; // what about 00 01 02 03 04 player??

                if (ValidationByte.ToString("X2").Substring(1, 1) != "4" && ValidationByte.ToString("X2").Substring(1, 1) != "C") // players will follow, but like 00 01 02 03 04 player [Haven't figured this out yet]
                {
                    Success = false;
                    return;
                }

                // Extract players
                // A non-followup packet will start with 11 bytes Between 24 0x00 and lvl/prestige, then 3x 12 bytes, 1x 11 bytes, 3x 12 bytes, ect
                while ((buffer.Length - mMW2PayloadOffset) >= 73) // 73 is the minimum amount of bytes needed for a packet without a playername
                {
                    temp = new Byte[buffer.Length - mMW2PayloadOffset];
                    Buffer.BlockCopy(buffer, mMW2PayloadOffset, temp, 0, temp.Length);
                    // 0, 4, 8, 12, ect shortend
                    // so index==0 || ((index%4)==0) > shortend
                    bool shortend;
                    if (playerIndex == 0 || ((playerIndex % 4) == 0))
                        shortend = true;
                    else
                        shortend = false;
                    PckPartystatePlayer p = new PckPartystatePlayer(temp, false, shortend, HostIP_ext);
                    Players.Add(p);

                    // increment playerindex and mw2payloadoffset
                    playerIndex++;
                    mMW2PayloadOffset += p.PlayerPayloadLength;
                }
            }
        }
    }
}
