using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Media_Player.ViewModel
{
    using BaseClass;
    using Media_Player.Model;

    public class AddTrack : ViewModelBase
    {

        #region Properties
        private string? trackName;
        public string? TrackName
        {
            get
            {
                return trackName;
            }
            set
            {
                trackName = value;
                onPropertyChanged(nameof(TrackName));
            }
        }

        private string? artist;
        public string? Artist
        {
            get
            {
                return artist;
            }
            set
            {
                artist = value;
                onPropertyChanged(nameof(Artist));
            }
        }

        private string? album;
        public string? Album
        {
            get
            {
                return album;
            }
            set
            {
                album = value;
                onPropertyChanged(nameof(Album));
            }
        }

        private string? genre;
        public string? Genre
        {
            get
            {
                return genre;
            }
            set
            {
                genre = value;
                onPropertyChanged(nameof(Genre));
            }
        }

        private string? releaseYear;
        public string? ReleaseYear
        {
            get
            {
                return releaseYear;
            }
            set
            {
                releaseYear = value;
                onPropertyChanged(nameof(ReleaseYear));
            }
        }

        private string? audiofilePath;
        public string? AudioFilePath
        {
            get
            {
                return audiofilePath;
            }
            set
            {
                audiofilePath = value;
                onPropertyChanged(nameof(AudioFilePath));
            }
        }
        #endregion

        #region Methods
        private void pickTrackFile()
        {
            try
            {
                string filePath = string.Empty;

                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "Audio (*.mp3, *.wav, .wma, .aac)|*.mp3;*.wav;*.wma;*.aac|All files (*.*)|*.*";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == true)
                {
                    filePath = dialog.FileName;
                    if (filePath != string.Empty)
                    {
                        AudioFilePath = filePath;
                        var tfile = TagLib.File.Create(AudioFilePath);

                        TrackName = tfile.Tag.Title;
                        if(tfile.Tag.Performers.Count() > 0)
                        {
                            Artist = tfile.Tag.Performers[0];
                        }
                        Album = tfile.Tag.Album;
                        if (tfile.Tag.Genres.Count() > 0)
                        {
                            Genre = tfile.Tag.Genres[0];
                        }
                        if (tfile.Tag.Year != 0)
                        {
                            ReleaseYear = tfile.Tag.Year.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AudioFilePath = null;
                MessageBox.Show($"Błąd podczas otwierania pliku:\n{ex.Message}", "Błąd odczytu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void clearForm()
        {
            TrackName = null;
            Artist = null;
            Album = null;
            Genre = null;
            ReleaseYear = null;
            AudioFilePath = null;
        }

        private void addTrackToPlaylist()
        {
            try
            {
                int? year = null;
                if (ReleaseYear != null)
                {
                    year = Int32.Parse(ReleaseYear);
                }
                Playlist? playlist = ViewModelShare.playlistShare;
                Track newTrack = new Track(null, TrackName!, AudioFilePath!);
                newTrack.Artist = artist;
                newTrack.Album = album;
                newTrack.Genre = genre;
                newTrack.ReleaseYear = year;
                playlist!.addTrack(newTrack);

                clearForm();
                MessageBox.Show($"Pomyślnie dodano nowy utwór.", "Sukces!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                clearForm();
                MessageBox.Show($"Błąd podczas zapisywania utworu:\n{ex.Message}", "Błąd zapisu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        #region Commands
        public ICommand PickFile
        {
            get
            {
                return new RelayCommand(execute => pickTrackFile(), canExecute => true);
            }
        }

        public ICommand AddNewTrack
        {
            get
            {
                return new RelayCommand(execute => addTrackToPlaylist(), canExecute => (TrackName != null && AudioFilePath != null));
            }
        }
        #endregion
    }
}
