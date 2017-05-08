using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace MW2KTCore
{
    public static class Steam
    {
        public static BitmapImage GetPlayerAvatar(UInt64 steamID)
        {
            string imgurl = "http://cdn.akamai.steamstatic.com/steamcommunity/public/images/avatars/fe/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_full.jpg";  // Deafault Steam avatar
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead("http://steamcommunity.com/profiles/" + steamID);
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                string line = reader.ReadLine();
                while (!line.Contains("playerAvatarAutoSizeInner"))
                    line = reader.ReadLine();
                // This is now the last read line:
                // <div class="playerAvatarAutoSizeInner"><img src="http://cdn.akamai.steamstatic.com/steamcommunity/public/images/avatars/48/48e0836bb433ebc476b3a06519f458e2cec45dfc_full.jpg"></div>
                int iAfterFirstQuote = line.IndexOf("src=") + 5;
                int iSecondQuote = line.IndexOf('"', iAfterFirstQuote);
                imgurl = line.Substring(iAfterFirstQuote, iSecondQuote - iAfterFirstQuote);
            }

            return new BitmapImage(new Uri(imgurl)); ;
        }

        public static BitmapImage GetCountryFlag(UInt64 steamID)
        {
            string imgurl = "http://cdn.akamai.steamstatic.com/steamcommunity/public/images/avatars/fe/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_full.jpg";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead("http://steamcommunity.com/profiles/" + steamID);
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                string line = reader.ReadLine();
                while(!String.IsNullOrEmpty(line))
                {
                    if(line.Contains("profile_flag"))
                    {
                        // This is now the last read line:
                        //< img class="profile_flag" src="http://steamcommunity-a.akamaihd.net/public/images/countryflags/nl.gif">
                        int iAfterFirstQuote = line.IndexOf("src=") + 5;
                        int iSecondQuote = line.IndexOf('"', iAfterFirstQuote);
                        imgurl = line.Substring(iAfterFirstQuote, iSecondQuote - iAfterFirstQuote);
                        // We've found our image, so break out
                        break;  // Is this back programming?
                    }
                    line = reader.ReadLine();
                }
            }
            return new BitmapImage(new Uri(imgurl));
        }
    }
}
