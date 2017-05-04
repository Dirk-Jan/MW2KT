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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MW2KT_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UI.MW2Label mLabel;
        public MainWindow()
        {
            InitializeComponent();

            mLabel = new UI.MW2Label();
            mLabel.VerticalAlignment = VerticalAlignment.Center;
            mLabel.HorizontalAlignment = HorizontalAlignment.Center;
            grdMain.Children.Add(mLabel);
            tboxInput.TextChanged += TboxInput_TextChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //AddColoredLabel();
            //PlayerView p = new PlayerView("Een ^4p^1layer ^3naam");
            //grdMain.Children.Add(p);
            
        }

        private void TboxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            mLabel.Text = tboxInput.Text;
        }

        private void AddColoredLabel()
        {
            Label l = new Label();
            StackPanel s = new StackPanel();
            s.Orientation = Orientation.Horizontal;
            TextBlock tb = new TextBlock();
            tb.Foreground = new SolidColorBrush(Colors.Red);
            TextBlock tb2 = new TextBlock();
            tb.Text = "Do";
            tb2.Text = "ei";
            s.Children.Add(tb);
            s.Children.Add(tb2);
            //this.AddChild(l);
            grdMain.Children.Add(s);
        }
    }
}
