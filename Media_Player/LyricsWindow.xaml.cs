using System.Windows;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for LyricsWindow.xaml
    /// </summary>
    public partial class LyricsWindow : Window
    {
        public LyricsWindow(string? Artist, string? Title, string lyrics)
        {
            InitializeComponent();
            // Replace \n and \r with the environment's newline character
            string formattedLyrics = lyrics.Replace("\\n", Environment.NewLine).Replace("\\r", "");
            LyricsTextBlock.Text = formattedLyrics;
            if (Artist != null && Title != null && Artist != String.Empty && Title != String.Empty)
            {
                LyricWindow.Title = $"Tekst utworu: {Artist} - {Title}";
            }
        }
    }
}
