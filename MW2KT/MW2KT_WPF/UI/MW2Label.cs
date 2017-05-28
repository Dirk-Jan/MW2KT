using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MW2KT_WPF.UI
{
    public class MW2Label : StackPanel
    {
        /*private string mText = "";
        public string Text
        {
            get { return mText; }
            set
            {
                mText = value;
                DrawLabel();
            }
        }*/

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                typeof(string),
                typeof(MW2Label),
                new PropertyMetadata(string.Empty));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
                DrawLabel();
                //Text = value;
            }
        }

        

        private double mFontSize = 24;
        public double FontSize
        {
            get { return mFontSize; }
            set
            {
                mFontSize = value;
                DrawLabel();
            }
        }

        public MW2Label()
        {
            this.DataContext = this;
            Orientation = Orientation.Horizontal;
            //Background = Brushes.Gray;
            DrawLabel();
            this.SizeChanged += MW2Label_SizeChanged;
        }

        private void MW2Label_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            //FontSize = this.ActualHeight * .75;
        }

        public void DrawLabel()
        {
            this.Children.Clear();

            // Draw the string
            List<int> iColorCodes = GetIndexesColorCodes();

            //string drawnText = string.Empty;
            //string drawnText = "r";
            // Draw everything in front of the first color code
            if (iColorCodes.Count == 0)  // There is no color coding, so draw the complete string
            {
                TextBlock tb = new TextBlock()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    FontSize = this.FontSize,
                    FontFamily = new FontFamily("Courier New"),
                    Text = this.Text
                };
                Children.Add(tb);
            }
            else if (iColorCodes[0] > 0) // There is text in front of the first color code
            {
                string str = this.Text.Substring(0, iColorCodes[0]);
                TextBlock tb = new TextBlock()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    FontSize = this.FontSize,
                    FontFamily = new FontFamily("Courier New"),
                    Text = str
                };
                Children.Add(tb);
            }


            // Draw everything after each color code
            for (int i = 0; i < iColorCodes.Count; i++)
            {
                int startIndex = iColorCodes[i] + 2;   // + 2 To skip the color coding ^1, ^2, ^3, ect
                int strLength = i != (iColorCodes.Count - 1) ? iColorCodes[i + 1] - startIndex : Text.Length - startIndex;
                string str = this.Text.Substring(startIndex, strLength);
                TextBlock tb = new TextBlock()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Foreground = GetColor(Convert.ToInt16(this.Text.Substring(iColorCodes[i] + 1, 1))),
                    FontSize = this.FontSize,
                    FontFamily = new FontFamily("Courier New"),
                    Text = str
                };
                Children.Add(tb);
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
                if (int.TryParse(this.Text[index + 1].ToString(), out int i))
                    colorCodes.Add(index);
                index = this.Text.IndexOf('^', index + 1);  // Do + 1, otherwise it'll see the last seen carat again
            }
            return colorCodes;
        }
    }
}
