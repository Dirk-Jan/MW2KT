using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MW2KTCore
{
    public class Player
    {
        private static readonly string mRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static readonly string mPlayersPath = mRootPath + @"/players/";
        private static readonly byte[] mValidationBytes = { 0x64, 0x6A };
        private static readonly int mPlayerAsFileByteCount = 15;

        public UInt64 SteamId { get; set; }
        public PlayerTag Tag { get; set; }
        public IPAddress IPAddress { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string PrestigeURL { get; set; }
        public bool IsHost { get; set; }

        public byte[] PlayerBuffer { get; set; }        // For debugging


        public Player(UInt64 steamId, PlayerTag tag, IPAddress ip, string name)
        {
            SteamId = steamId;
            Tag = tag;
            IPAddress = ip;
            Name = name;
        }

        public Player(Packets.PckPartystatePlayer p)
        {
            PlayerBuffer = p.PacketBuffer;
            IsHost = p.IsHost;
            SteamId = p.SteamID;
            Name = p.PlayerNameWithoutColorCodes;
            Level = p.PlayerLevel.ToString();
            PrestigeURL = GetPrestigeSymbolURI(p.PlayerPrestigeLevel, p.PlayerLevel);
            IPAddress = p.ExternalIP;
            LoadPlayerTagFromFile();
        }

        #region GUI Related
        private string GetPrestigeSymbolURI(int prestige, int level)
        {
            //level++;                                                // Increment level by 1 so the switch statement is correct
            string uri = "pack://application:,,,/Resources/";
            switch (prestige)
            {
                case 0:
                    switch (level)
                    {
                        case 1:
                        case 2:
                        case 3:
                            uri += "l1_2_3.png";
                            break;
                        case 4:
                        case 5:
                        case 6:
                            uri += "l4_5_6.png";
                            break;
                        case 7:
                        case 8:
                        case 9:
                            uri += "l7_8_9.png";
                            break;
                        case 10:
                        case 11:
                        case 12:
                            uri += "l10_11_12.png";
                            break;
                        case 13:
                        case 14:
                        case 15:
                            uri += "l13_14_15.png";
                            break;
                        case 16:
                        case 17:
                        case 18:
                            uri += "l16_17_18.png";
                            break;
                        case 19:
                        case 20:
                        case 21:
                            uri += "l19_20_21.png";
                            break;
                        case 22:
                        case 23:
                        case 24:
                            uri += "l22_23_24.png";
                            break;
                        case 25:
                        case 26:
                        case 27:
                            uri += "l25_26_27.png";
                            break;
                        case 28:
                        case 29:
                        case 30:
                            uri += "l28_29_30.png";
                            break;
                        case 31:
                        case 32:
                        case 33:
                            uri += "l31_32_33.png";
                            break;
                        case 34:
                        case 35:
                        case 36:
                            uri += "l34_35_36.png";
                            break;
                        case 37:
                        case 38:
                        case 39:
                            uri += "l37_38_39.png";
                            break;
                        case 40:
                        case 41:
                        case 42:
                            uri += "l40_41_42.png";
                            break;
                        case 43:
                        case 44:
                        case 45:
                            uri += "l43_44_45.png";
                            break;
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                            uri += "l46_47_48_49.png";
                            break;
                        case 50:
                        case 51:
                        case 52:
                        case 53:
                            uri += "l50_51_52_53.png";
                            break;
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                            uri += "l54_55_56_57.png";
                            break;
                        case 58:
                        case 59:
                        case 60:
                        case 61:
                            uri += "l58_59_60_61.png";
                            break;
                        case 62:
                        case 63:
                        case 64:
                        case 65:
                            uri += "l62_63_64_65.png";
                            break;
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                            uri += "l46_47_48_49.png";
                            break;
                        case 70:
                            uri += "l70.png";
                            break;
                        default:
                            uri += "cross.png";
                            break;
                    }
                    break;
                case 1:
                    uri += "p1.png";
                    break;
                case 2:
                    uri += "p2.png";
                    break;
                case 3:
                    uri += "p3.png";
                    break;
                case 4:
                    uri += "p4.png";
                    break;
                case 5:
                    uri += "p5.png";
                    break;
                case 6:
                    uri += "p6.png";
                    break;
                case 7:
                    uri += "p7.png";
                    break;
                case 8:
                    uri += "p8.png";
                    break;
                case 9:
                    uri += "p9.png";
                    break;
                case 10:
                    uri += "p10.png";
                    break;
                default:
                    uri += "cross.png";
                    break;
            }
            return uri;
        }
        #endregion
        #region Tag Related
        private void LoadPlayerTagFromFile()
        {
            var fileName = mRootPath + @"/players/" + SteamId.ToString() + ".mw2kt_player";
            if (File.Exists(fileName))
            {
                var player = LoadPlayerFromFile(fileName);
                if (player != null)
                    Tag = player.Tag;
            }
        }

        public static List<Player> LoadRegisteredPlayersFromFile()
        {
            var players = new List<Player>();
            if (Directory.Exists(mPlayersPath))
            {
                string[] files = Directory.GetFiles(mRootPath + @"/players/");
                foreach (var file in files)
                {
                    if (Path.GetExtension(file) == ".mw2kt_player")
                    {
                        var player = LoadPlayerFromFile(file);
                        if (player != null)
                            players.Add(player);
                    }
                }

            }
            return players;
        }

        private static Player LoadPlayerFromFile(string fileName)
        {
            byte[] buffer = File.ReadAllBytes(fileName);
            if (buffer.Length >= mPlayerAsFileByteCount)                 // Check if we have enough bytes in the file
            {
                bool fileIsValid = true;
                for (int i = 0; i < mValidationBytes.Length; i++)       // Check if the validation bytes are correct
                    if (buffer[i] != mValidationBytes[i])
                        fileIsValid = false;
                if (fileIsValid)
                {
                    int bytesRead = mValidationBytes.Length;

                    var steamId = BitConverter.ToUInt64(buffer, bytesRead);
                    bytesRead += 8;

                    var tag = (PlayerTag)buffer[bytesRead];
                    bytesRead += 1;

                    var ip_array = new byte[4];
                    Buffer.BlockCopy(buffer, bytesRead, ip_array, 0, 4);
                    var ip = new IPAddress(ip_array);
                    bytesRead += 4;

                    var name_array = new byte[buffer.Length - bytesRead];
                    Buffer.BlockCopy(buffer, bytesRead, name_array, 0, name_array.Length);
                    var name = Encoding.UTF8.GetString(name_array);

                    return new Player(steamId, tag, ip, name);
                }
            }
            return null;
        }

        public bool SavePlayerToFile()
        {
            bool success = true;
            try
            {
                if (!Directory.Exists(mPlayersPath))
                    Directory.CreateDirectory(mPlayersPath);

                string file = mPlayersPath + SteamId.ToString() + ".mw2kt_player";
                File.WriteAllBytes(file, ToByteArray());
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        private byte[] ToByteArray()
        {
            var array = new byte[mPlayerAsFileByteCount + Name.Length + 1];
            int bytesCopied = 0;

            Buffer.BlockCopy(mValidationBytes, 0, array, bytesCopied, mValidationBytes.Length);
            bytesCopied += mValidationBytes.Length;

            var steamId = BitConverter.GetBytes(SteamId);
            Buffer.BlockCopy(steamId, 0, array, bytesCopied, steamId.Length);
            bytesCopied += steamId.Length;

            array[bytesCopied] = (byte)Tag;
            bytesCopied += 1;

            var ip = IPAddress.GetAddressBytes();
            Buffer.BlockCopy(ip, 0, array, bytesCopied, ip.Length);
            bytesCopied += ip.Length;

            var name = Encoding.UTF8.GetBytes(Name);                        // Unicode?? always two bytes? sometimes two, sometimes 1?
            Buffer.BlockCopy(name, 0, array, bytesCopied, name.Length);
            bytesCopied += name.Length;
            array[bytesCopied] = 0x00;
            bytesCopied += 1;

            return array;
        }
        #endregion
    }
}
