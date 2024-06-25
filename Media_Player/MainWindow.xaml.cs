using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool sliderDragged = false;
        private string durationSting = "00:00";
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((MediaElement.Source != null) && (MediaElement.NaturalDuration.HasTimeSpan) && (!sliderDragged))
            {
                ProgressSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = MediaElement.Position.TotalSeconds;
                durationSting = TimeSpan.FromSeconds(MediaElement.NaturalDuration.TimeSpan.TotalSeconds).ToString(@"mm\:ss");
            }
        }

        private void ProgressSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            sliderDragged = true;
        }

        private void ProgressSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            sliderDragged = false;
            MediaElement.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProgressLabel.Content = TimeSpan.FromSeconds(ProgressSlider.Value).ToString(@"mm\:ss")+"/"+durationSting;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("play");
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