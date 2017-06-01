using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MW2KTCore.Packets
{
    public class PckPartystatePlayer // 62 + 4/3 + 8/7 so either 74 or 73
    {
        public Byte PlayerID { get; set; }              // 1 byte
        // byte                                         // 1 byte
        // byte                                         // 1 byte
        // byte                                         // 1 byte
        public String PlayerName { get; set; }          // null terminated
        // unknown byte[4] always 0                     // 4 bytes
        public UInt64 SteamID { get; set; }             // 8 bytes
        public IPAddress InternalIP { get; set; }       // 4 bytes
        public IPAddress ExternalIP { get; set; }       // 4 bytes
        public UInt16 InternalPort { get; set; }        // 2 bytes
        public UInt16 ExternalPort { get; set; }        // 2 bytes
        // Unknown byte[24] always 0                    // 24 bytes 
        public UInt64 PartyID { get; set; }             // 8 bytes
        // unknown byte[4] or byte[3]                   // 4/3 bytes non-followup
        public Byte PlayerLevel { get; set; }           // 1 byte
        public Byte PlayerPrestigeLevel { get; set; }   // 1 byte
        // Unknown byte[8] or byte[7]                   // 8/7 bytes followup

        public bool IsHost { get; set; }

        public int PlayerPayloadLength { get; set; }
        public string PlayerNameWithoutColorCodes
        {
            get
            {
                string name = PlayerName;
                for (int i = 0; i < name.Length; i++)
                {
                    if(i > 0 && name[i-1] == '^' && int.TryParse(name[i].ToString(), out int x))
                    {
                        name = name.Remove(i - 1, 2);
                        i = i-1;    // Step back one place, it'll continue with a char that hasn't been checked yet
                    }
                }
                return name;
            }
        }

        // Constructor
        public PckPartystatePlayer(byte[] buffer, bool followUpPackage, bool shortened, IPAddress hostIp)
        {
            PlayerID = buffer[0];
            PlayerName = ReadNullTerminatedString(buffer, 4);
            int playerNameOffset = 4 + PlayerName.Length + 1;
            SteamID = BitConverter.ToUInt64(buffer, playerNameOffset + 4);
            Byte[] temp = new Byte[4];
            Buffer.BlockCopy(buffer, playerNameOffset + 12, temp, 0, 4);
            InternalIP = new IPAddress(temp);
            Buffer.BlockCopy(buffer, playerNameOffset + 16, temp, 0, 4);
            ExternalIP = new IPAddress(temp);
            InternalPort = BitConverter.ToUInt16(buffer, playerNameOffset + 20);
            ExternalPort = BitConverter.ToUInt16(buffer, playerNameOffset + 22);
            PartyID = BitConverter.ToUInt64(buffer, playerNameOffset + 48);
            int playerLevelOffset;
            if (followUpPackage)
            {
                playerLevelOffset = playerNameOffset + 56 + 4;
            }
            else
            {
                if (shortened)
                    playerLevelOffset = playerNameOffset + 56 + 3;
                else
                    playerLevelOffset = playerNameOffset + 56 + 4;
            }
            PlayerLevel = (byte)((int)buffer[playerLevelOffset] + 1);
            PlayerPrestigeLevel = buffer[playerLevelOffset + 1];

            // Set amount of bytes read / used
            PlayerPayloadLength = (shortened ? 73 : 74) + PlayerName.Length + 1;

            if (hostIp == null || !hostIp.Equals(ExternalIP))
                IsHost = false;
            else
                IsHost = true;
              
        }

        private string ReadNullTerminatedString(byte[] buffer, int offset)
        {
            List<Byte> bytes = new List<Byte>();
            Byte lastReadByte = buffer[offset];
            while (offset < buffer.Length && lastReadByte != 0x00)
            {
                bytes.Add(lastReadByte);
                offset++;
                lastReadByte = buffer[offset];
            }
            return System.Text.Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}
