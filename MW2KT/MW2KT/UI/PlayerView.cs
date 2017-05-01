using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MW2KT.UI
{
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
            //SetKills(0);
            //SetDeaths(0);
            //SetTeam("spectator");
            this.BackColor = Color.Gray;
        }

        private void lblBlock_Click(object sender, EventArgs e)
        {
            lblBlock.Text = "[Blocked IP Address]";
        }

        public void SetName(string name)
        {
            if (lblName.InvokeRequired)
            {
                lblName.Invoke((MethodInvoker)delegate ()
                {
                    SetName(name);
                });
            }
            else lblName.Text = name;
        }

        public void SetIPAddress(string ipAddress)
        {
            if (lblIP.InvokeRequired)
            {
                lblIP.Invoke((MethodInvoker)delegate ()
                {
                    SetIPAddress(ipAddress);
                });
            }
            else lblIP.Text = ipAddress;
        }

        public void SetRank(int prestige, int level)
        {
            Image img = null;
            switch (prestige)
            {
                case 0:
                    switch (level)
                    {
                        case 1:
                        case 2:
                        case 3:
                            img = MW2KT.Properties.Resources._1_2_3;
                            break;
                        case 4:
                        case 5:
                        case 6:
                            img = MW2KT.Properties.Resources._4_5_6;
                            break;
                        case 7:
                        case 8:
                        case 9:
                            img = MW2KT.Properties.Resources._7_8_9;
                            break;
                        case 10:
                        case 11:
                        case 12:
                            img = MW2KT.Properties.Resources._10_11_12;
                            break;
                        case 13:
                        case 14:
                        case 15:
                            img = MW2KT.Properties.Resources._13_14_15;
                            break;
                        case 16:
                        case 17:
                        case 18:
                            img = MW2KT.Properties.Resources._16_17_18;
                            break;
                        case 19:
                        case 20:
                        case 21:
                            img = MW2KT.Properties.Resources._19_20_21;
                            break;
                        case 22:
                        case 23:
                        case 24:
                            img = MW2KT.Properties.Resources._22_23_24;
                            break;
                        case 25:
                        case 26:
                        case 27:
                            img = MW2KT.Properties.Resources._25_26_27;
                            break;
                        case 28:
                        case 29:
                        case 30:
                            img = MW2KT.Properties.Resources._28_29_30;
                            break;
                        case 31:
                        case 32:
                        case 33:
                            img = MW2KT.Properties.Resources._31_32_33;
                            break;
                        case 34:
                        case 35:
                        case 36:
                            img = MW2KT.Properties.Resources._34_35_36;
                            break;
                        case 37:
                        case 38:
                        case 39:
                            img = MW2KT.Properties.Resources._37_38_39;
                            break;
                        case 40:
                        case 41:
                        case 42:
                            img = MW2KT.Properties.Resources._40_41_42;
                            break;
                        case 43:
                        case 44:
                        case 45:
                            img = MW2KT.Properties.Resources._43_44_45;
                            break;
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                            img = MW2KT.Properties.Resources._46_47_48_49;
                            break;
                        case 50:
                        case 51:
                        case 52:
                        case 53:
                            img = MW2KT.Properties.Resources._50_51_52_53;
                            break;
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                            img = MW2KT.Properties.Resources._54_55_56_57;
                            break;
                        case 58:
                        case 59:
                        case 60:
                        case 61:
                            img = MW2KT.Properties.Resources._58_59_60_61;
                            break;
                        case 62:
                        case 63:
                        case 64:
                        case 65:
                            img = MW2KT.Properties.Resources._62_63_64_65;
                            break;
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                            img = MW2KT.Properties.Resources._46_47_48_49;
                            break;
                        case 70:
                            img = MW2KT.Properties.Resources._70;
                            break;
                    }
                    break;
                case 1:
                    img = MW2KT.Properties.Resources.p1;
                    break;
                case 2:
                    img = MW2KT.Properties.Resources.p2;
                    break;
                case 3:
                    img = MW2KT.Properties.Resources.p3;
                    break;
                case 4:
                    img = MW2KT.Properties.Resources.p4;
                    break;
                case 5:
                    img = MW2KT.Properties.Resources.p5;
                    break;
                case 6:
                    img = MW2KT.Properties.Resources.p6;
                    break;
                case 7:
                    img = MW2KT.Properties.Resources.p7;
                    break;
                case 8:
                    img = MW2KT.Properties.Resources.p8;
                    break;
                case 9:
                    img = MW2KT.Properties.Resources.p9;
                    break;
                case 10:
                    img = MW2KT.Properties.Resources.p10;
                    break;
            }
            SetLevel(level);
            SetPrestige(img);
        }

        private void SetLevel(int level)
        {
            if (lblLevel.InvokeRequired)
            {
                lblLevel.Invoke((MethodInvoker)delegate ()
                {
                    SetLevel(level);
                });
            }
            else
            {
                lblLevel.Text = level.ToString();
            }
        }
        private void SetPrestige(Image img)
        {
            if (pboxIcon.InvokeRequired)
            {
                pboxIcon.Invoke((MethodInvoker)delegate ()
                {
                    SetPrestige(img);
                });
            }
            else pboxIcon.Image = img;
        }

        public void SetTeam(string team)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    SetTeam(team);
                });
            }
            else
            {
                Color c;
                if (team == "axis")
                    c = Color.Coral;
                else if (team == "allies")
                    c = Color.SlateBlue;
                else
                    c = Color.Wheat;
                this.BackColor = c;
            }
        }

        public void SetHost(bool isHost)
        {
            if (lblIP.InvokeRequired)
            {
                lblIP.Invoke((MethodInvoker)delegate ()
                {
                    SetHost(isHost);
                });
            }
            else
            {
                if (isHost)
                    lblIP.BackColor = Color.Gainsboro;
                else
                    lblIP.BackColor = this.BackColor;
            }
        }

        public void SetViewLocation(Point p)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    SetViewLocation(p);
                });
            }
            else this.Location = p;
        }

        public void SetPartyImage(Image img)
        {
            if (pboxPartyIcon.InvokeRequired)
            {
                pboxPartyIcon.Invoke((MethodInvoker)delegate ()
                {
                    SetPartyImage(img);
                });
            }
            else
                pboxPartyIcon.Image = img;
        }
    }
}
