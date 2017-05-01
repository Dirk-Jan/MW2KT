using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MW2KT
{
    public enum PlayerTag
    {
        None,
        Cheater
    }
    public class Player
    {
        public string PlayerName { get; set; }
        public UInt64 SteamID { get; set; }
        public PlayerTag Tag { get; set; }
        public IPAddress IPAddress { get; set; }
    }
}
