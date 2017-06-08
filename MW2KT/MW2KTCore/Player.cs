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

        public Player(UInt64 steamId, PlayerTag tag, IPAddress ip, string name)
        {
            SteamId = steamId;
            Tag = tag;
            IPAddress = ip;
            Name = name;
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
                        byte[] buffer = File.ReadAllBytes(file);
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

                                players.Add(new Player(steamId, tag, ip, name));
                            }
                        }
                    }
                }

            }
            return players;
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
    }
}
