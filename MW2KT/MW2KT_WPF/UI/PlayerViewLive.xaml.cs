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
            SetRank(0, 13);
        }

        private void ImgAvatar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/profiles/" + mSteamID);
        }

        public void SetRank(int prestige, int level)
        {
            BitmapImage img = null;
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
            }
            img = new BitmapImage(new Uri(uri));
            SetLevel(level);
            SetPrestige(img);
        }

        private void SetLevel(int level)
        {
            tbLevel.Text = level.ToString();
        }
        private void SetPrestige(BitmapImage img)
        {
            imgAvatar.Source = img;
        }
    }
}
