using MW2KT.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MW2KT
{
    public class PlayerOLD
    {
        private UInt64 mSteamID;
        private String mName;
        private UInt64 mPartyID;
        private IPAddress mIPAddress;
        private PlayerView mView;


        public PlayerOLD(UInt64 steamID, String name, UInt64 partyID, IPAddress ip, Byte playerLevel, Byte playerPrestige)
        {
            mView = new PlayerView();
            mSteamID = steamID;
            //Name = name;
            Name = steamID.ToString();
            mPartyID = partyID;
            IPAddress = ip;
            mView.SetRank(playerPrestige, playerLevel + 1);
        }

        public void SetPartyImage(Image img)
        {
            mView.SetPartyImage(img);
        }

        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                mView.SetName(mName);
            }
        }

        public IPAddress IPAddress
        {
            get { return mIPAddress; }
            set
            {
                mIPAddress = value;
                mView.SetIPAddress(mIPAddress.ToString());
            }
        }

        public UInt64 SteamID
        {
            get { return mSteamID; }
            set { mSteamID = value; }
        }

        public UInt64 PartyID { get { return mPartyID; } }

        public PlayerView View
        {
            get { return mView; }
        }
    }
}
