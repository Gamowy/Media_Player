using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Media_Player
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

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (MediaPlayerVM.MediaElementVM.IsPlaying)
            {
                MediaElement.Pause();
                MediaPlayerVM.MediaElementVM.IsPlaying = false;
            }
            else
            {
                MediaElement.Play();
                MediaPlayerVM.MediaElementVM.IsPlaying = true;
            }
        }
    }
}