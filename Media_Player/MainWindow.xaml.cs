using Media_Player.ViewModel;
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
        private string durationString = "00:00";
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            MediaPlayerVM.MediaElementVM.PlayRequest += (sender, e) => Play_Request();
            MediaPlayerVM.MediaElementVM.VolumeButtonUpdate += (sender, e) => Change_Volume_Button_Image();
        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            if ((MediaElement.Source != null) && (MediaElement.NaturalDuration.HasTimeSpan) && (!sliderDragged))
            {
                ProgressSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = MediaElement.Position.TotalSeconds;
                TimeSpan duration = TimeSpan.FromSeconds(MediaElement.NaturalDuration.TimeSpan.TotalSeconds);
                durationString = $"{(int)duration.TotalMinutes}:{duration.Seconds:00}";
                if (ProgressSlider.Value == ProgressSlider.Maximum)
                {
                    PlayButtonImg.Source = new BitmapImage(new Uri(@"/Media_Player;component/Resources/play.png", UriKind.Relative));
                    MediaElement.Pause();
                    MediaPlayerVM.MediaElementVM.IsPlaying = false;
                }
            }
            else if (MediaElement.Source == null)
            {
                ProgressSlider.Value = 0;
                ProgressLabel.Content = "00:00";
            }
        }

        private void ProgressSlider_DragStarted(object? sender, DragStartedEventArgs e)
        {
            sliderDragged = true;
        }

        private void ProgressSlider_DragCompleted(object? sender, DragCompletedEventArgs e)
        {
            sliderDragged = false;
            MediaElement.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }

        private void ProgressSlider_ValueChanged(object? sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan timeElapsed=TimeSpan.FromSeconds(ProgressSlider.Value);
            ProgressLabel.Content =$"{ (int)timeElapsed.TotalMinutes}:{timeElapsed.Seconds:00}"+"/"+durationString;
        }

        private void Play_Request()
        {
            if (MediaPlayerVM.MediaElementVM.IsPlaying)
            {
                MediaElement.Pause();
                MediaPlayerVM.MediaElementVM.IsPlaying = false;
                PlayButtonImg.Source = new BitmapImage(new Uri(@"/Media_Player;component/Resources/play.png", UriKind.Relative));
            }
            else
            {
                if(ProgressSlider.Value == ProgressSlider.Maximum)
                {
                    MediaElement.Position = TimeSpan.FromSeconds(0);
                }
                MediaElement.Play();
                MediaPlayerVM.MediaElementVM.IsPlaying = true;
                PlayButtonImg.Source = new BitmapImage(new Uri(@"/Media_Player;component/Resources/pause.png", UriKind.Relative));
            }
        }

        private void Change_Volume_Button_Image()
        {
            if (VolumeSlider.Value == 0)
            {
                VolumeButtonImg.Source = new BitmapImage(new Uri(@"/Media_Player;component/Resources/mute.png", UriKind.Relative));
            }
            else
            {
                VolumeButtonImg.Source = new BitmapImage(new Uri(@"/Media_Player;component/Resources/volume.png", UriKind.Relative));
            }
        }
    }
}