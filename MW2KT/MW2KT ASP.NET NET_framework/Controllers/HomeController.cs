using MW2KTCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MW2KT_ASP.NET_NET_framework.Controllers
{
    public class HomeController : Controller
    {
        private PacketHandler mPacketHandler = null;
        private List<Player> mPlayers = new List<Player>();

        public ActionResult Index(List<Player> players)
        {
            if (mPacketHandler == null)
            {
                PacketCapture.SelectedDevice = PacketCapture.AvailableDevicesExtended[PacketCapture.AvailableDevicesExtended.Count - 1];
                mPacketHandler = new PacketHandler();
                mPacketHandler.NewPlayerListAvailable += PacketHandler_NewPlayerListAvailable;

                var t = new Thread(new ParameterizedThreadStart(PacketCapture.StartCapturing));
                t.Start(mPacketHandler);
            }
            if (players == null)
                players = new List<Player>();
            return View(players);
        }

        protected void PacketHandler_NewPlayerListAvailable(List<MW2KTCore.Player> players)
        {
            //var newPlayers = new List<Player>();
            //foreach (var p in players)
            //    newPlayers.Add(new Player(p));
            //InvokeUpdatePlayerListInGUI(newPlayers);
            Player.GetParties(players);
            //var images = GetRandomizedPartyImages();
            /*foreach (var player in players)
            {
                if (player.PartyImageIndex < 0)
                    player.PartyImageName = null;
                else player.PartyImageName = "pack://application:,,,/Resources/" + images[player.PartyImageIndex];
            }*/
            //InvokeUpdatePlayerListInGUI(players);
            //mPlayers = players;
            //Response.Redirect(Request.RawUrl);
            RedirectToAction("Index", players);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
            //return View("Index", players);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}