using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MW2KTCore;
using System.Threading;

namespace MW2KT_ASP.NET.Controllers
{
    public class HomeController : Controller
    {
        PacketHandler mPacketHandler = null;
        public IActionResult Index()
        {
            if(mPacketHandler == null)
            {
                mPacketHandler = new PacketHandler();
                PacketCapture.SelectedDevice = (LivePacketDeviceExtended)e.AddedItems[0];
                mPacketHandler.NewPlayerListAvailable += PacketHandler_NewPlayerListAvailable;

                var t = new Thread(new ParameterizedThreadStart(PacketCapture.StartCapturing));
                t.Start(mPacketHandler);
            }

            return View();
        }

        protected void PacketHandler_NewPlayerListAvailable(List<MW2KTCore.Player> players)
        {
            //var newPlayers = new List<Player>();
            //foreach (var p in players)
            //    newPlayers.Add(new Player(p));
            //InvokeUpdatePlayerListInGUI(newPlayers);
            Player.GetParties(players);
            var images = GetRandomizedPartyImages();
            foreach (var player in players)
            {
                if (player.PartyImageIndex < 0)
                    player.PartyImageName = null;
                else player.PartyImageName = "pack://application:,,,/Resources/" + images[player.PartyImageIndex];
            }
            InvokeUpdatePlayerListInGUI(players);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
