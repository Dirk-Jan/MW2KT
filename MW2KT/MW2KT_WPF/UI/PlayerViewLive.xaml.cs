using MW2KTCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MW2KT_WPF.UI
{
    /// <summary>
    /// Interaction logic for PlayerViewLive.xaml
    /// </summary>
    public partial class PlayerViewLive : UserControl
    {
        private string mPlayerName;
        public string PlayerName
        {
            get { return mPlayerName; }
            set
            {
                mPlayerName = value;
                lblName.Text = value;
            }
        }

        private UInt64 mSteamID;
        public UInt64 SteamID
        {
            get { return mSteamID; }
            set
            {
                mSteamID = value;
                imgAvatar.Source = Steam.GetPlayerAvatar(value);
                //imgCountryFlag.Source = Steam.GetCountryFlag(value);
            }
        }

        public PlayerViewLive()
        {
            InitializeComponent();
            imgAvatar.MouseLeftButtonDown += ImgAvatar_MouseLeftButtonDown;
        }

        private void ImgAvatar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/profiles/" + mSteamID);
        }
    }
}
