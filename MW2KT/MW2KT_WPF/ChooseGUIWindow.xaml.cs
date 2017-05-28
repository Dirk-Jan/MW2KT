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
using System.Windows.Shapes;

namespace MW2KT_WPF
{
    /// <summary>
    /// Interaction logic for ChooseGUIWindow.xaml
    /// </summary>
    public partial class ChooseGUIWindow : Window
    {
        public ChooseGUIWindow()
        {
            InitializeComponent();
        }

        private void btnScoreboardGUI_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }

        private void btnTableGUI_Click(object sender, RoutedEventArgs e)
        {
            new TableGUIWindow().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mw2lbl.Text = "^6G^3host^2~";
        }
    }
}
