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
            if (Artist != null && Title != null) 
            {
                LyricWindow.Title = $"Tekst utworu: {Title} - {Artist}";
            }
         }
    }
}
