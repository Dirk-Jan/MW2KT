using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MW2KT.UI
{
    public class PlayerListItem : Panel // Can I cast the player object to playerlistitem and have its properties?
    {
        /// <summary>
        /// This PlayerListItem will only contain the playername, link to Steam profile, IP address and player tag.
        /// This item is used in the registered playerlist.
        /// This control has the ability to be marked / selected.
        /// </summary>
        /// <param name="p">Player</param>
        public PlayerListItem(Player p)
        {
            // PlayerName label
            Mw2Label lblName = new Mw2Label();
            lblName.Text = p.PlayerName;
            
        }
    }
}
