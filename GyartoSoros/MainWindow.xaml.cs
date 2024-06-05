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

namespace GyartoSoros
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void KovetkezoKor_Click(object sender, RoutedEventArgs e)
        {
            int meret = 8;
            if (TablaMeret.SelectedIndex == 0)
            {
                meret = 8;
            }
            if (TablaMeret.SelectedIndex == 1)
            {
                meret = 12;
            }
            if (TablaMeret.SelectedIndex == 2)
            {
                meret = 16;
            }
            for (int i = 0; i < meret; i++)
            {
                Rectangle doboz = new Rectangle();
                doboz.Height = 100;
                doboz.Width = 100;
                doboz.Fill = new SolidColorBrush(Colors.Blue);
                doboz.Margin = new Thickness(10, 10, 10, 10);
                Grd_2.Children.Add(doboz);
            }
            for (int i = 0; i < meret; i++)
            {
                Rectangle doboz = new Rectangle();
                doboz.Height = 100;
                doboz.Width = 100;
                doboz.Fill = new SolidColorBrush(Colors.Red);
                doboz.Margin = new Thickness(10, 10, 10, 10);
                Grd_1.Children.Add(doboz);
            }
        }
    }
}
