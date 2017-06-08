using MW2KTCore;
using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Test_Pcap
{
    class Program
    {
        static void Main(string[] args)
        {
            var player = new Player(0321125326, PlayerTag.Cheater, new System.Net.IPAddress(new byte[] { 0xC0, 0xA5, 0xDD, 0x88 }), "-");
            player.SavePlayerToFile();

            var players = Player.LoadRegisteredPlayersFromFile();
            foreach(var p in players)
            {
                Console.WriteLine("====================================================");
                Console.WriteLine("SteamID: " + p.SteamId.ToString());
                Console.WriteLine("PlayerTag: " + p.Tag.ToString());
                Console.WriteLine("IP Address: " + p.IPAddress.ToString());
                Console.WriteLine("Name: " + p.Name);
                Console.WriteLine();
            }

            Console.ReadLine();
        }

    }
}
