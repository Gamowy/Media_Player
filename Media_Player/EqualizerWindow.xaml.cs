using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Media_Player
{
    /// <summary>
    /// Logika interakcji dla klasy EqualizerWindow.xaml
    /// </summary>
    public partial class EqualizerWindow : Window
    {
        public EqualizerWindow(string filename)
        {
            InitializeComponent();
            EqualizerVM.Filename = filename;
            EqualizerVM.Start();
            EqualizerVM.Close += (sender, e) => this.Close();
        }

        private void EqWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EqualizerVM.Stop();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            int index = int.Parse(slider.Tag.ToString());
            EqualizerVM.ChangeFilter(index, slider.Value);
        }
    }
}
