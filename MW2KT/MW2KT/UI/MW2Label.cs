using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2KT.UI
{
    public class Mw2Label : System.Windows.Forms.Control
    {
        private string _text;
        public string Text2
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                // Force redraw to show new color in designer instantly
                if (DesignMode)
                    Invalidate();
                Refresh();
            }
        }

        public Mw2Label()
        {
            this.Paint += MW2Label_Paint;
        }

        private void MW2Label_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Draw the string
            //e.Graphics.DrawString(this.Text, this.Font, System.Drawing.Brushes.Black, new System.Drawing.PointF(0, 0));
            List<int> iColorCodes = GetIndexesColorCodes();

            //string drawnText = string.Empty;
            string drawnText = "r";
            // Draw everything in front of the first color code
            if (iColorCodes.Count == 0)  // There is no color coding, so draw the complete string
            {
                e.Graphics.DrawString(this.Text, this.Font, Brushes.White, 0, 0);
            }
            else if (iColorCodes[0] > 0) // There is text in front of the first color code
            {
                string str = this.Text.Substring(0, iColorCodes[0]);
                e.Graphics.DrawString(str, this.Font, Brushes.White, 0, 0);
                drawnText += str;
            }

            StringFormat strFormat = new StringFormat();
            strFormat.Trimming = StringTrimming.Character;

            // Draw everything after each color code
            for (int i = 0; i < iColorCodes.Count; i++)
            {
                int startIndex = iColorCodes[i] + 2;   // + 2 To skip the color coding ^1, ^2, ^3, ect
                int strLength = i != (iColorCodes.Count - 1) ? iColorCodes[i + 1] - startIndex : Text.Length - startIndex;
                string str = this.Text.Substring(startIndex, strLength);
                float width = e.Graphics.MeasureString(drawnText, this.Font).Width - e.Graphics.MeasureString("r", this.Font).Width;    // For some reason when I don't calculate it this way, I get a wierd margin after the first drawn string. Haven't figured this out yet.
                //float width = e.Graphics.MeasureString("e", this.Font).Width * drawnText.Length;
                e.Graphics.DrawString(str, this.Font, GetColor(Convert.ToInt16(this.Text.Substring(iColorCodes[i] + 1, 1))), width, 0);
                drawnText += str;
            }
        }

        private Brush GetColor(int colorCode)
        {
            switch (colorCode)
            {
                case 1:
                    return Brushes.Red;
                case 2:
                    return Brushes.Green;
                case 3:
                    return Brushes.Yellow;
                case 4:
                    return Brushes.Blue;
                case 5:
                    return Brushes.Cyan;
                case 6:
                    return Brushes.HotPink;
                case 7:
                    return Brushes.White;
                default:
                    return Brushes.Black;
            }
        }

        private List<int> GetIndexesColorCodes()
        {
            List<int> colorCodes = new List<int>();
            int index = this.Text.IndexOf('^');
            while (index != -1 && index + 1 <= this.Text.Length - 1)
            {
                char c = this.Text[index + 1];
                int i;
                if (int.TryParse(this.Text[index + 1].ToString(), out i))
                    colorCodes.Add(index);
                index = this.Text.IndexOf('^', index + 1);  // Do + 1, otherwise it'll see the last seen carat again
            }
            return colorCodes;
        }
    }
}
