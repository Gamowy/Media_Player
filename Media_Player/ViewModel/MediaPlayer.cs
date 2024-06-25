using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Media_Player.ViewModel
{
    using BaseClass;
    using Media_Player.Model;
    using System.Configuration;
    using System.Diagnostics.Tracing;
    using System.Windows;
    using System.Windows.Input;

    public enum PlayMode { None, Video, Playlist};

    public class MediaPlayer : ViewModelBase
    {
        public MediaPlayer()
        {
            MediaElementVM = new MediaElementViewModel();
            MediaPlayMode = PlayMode.None;
        }

        #region Properties

        private PlayMode playmode;
        public PlayMode MediaPlayMode 
        { 
            get
            {
                return playmode;
            }
            set
            {
                playmode = value;
                if (value == PlayMode.None || value == PlayMode.Playlist)
                {
                    MediaElementVM.MediaUri = null;
                }
                onPropertyChanged(nameof(PlayMode));
            }
        }
        
        public Playlist? playlist;

        private ObservableCollection<Track>? tracks = null;
        public ObservableCollection<Track>? Tracks
        {
            get
            {
                return tracks;
            }
            set
            {
                tracks = value;
                onPropertyChanged(nameof(Tracks));
            }
        }

        private Track? selectedTrack;
        public Track? SelectedTrack
        {
            get
            {
                return selectedTrack;
            }
            set
            {
                selectedTrack = value;
                onPropertyChanged(nameof(SelectedTrack));
            }
        }

        public MediaElementViewModel MediaElementVM { get; set; }
        #endregion

        #region Methods
        private void createPlaylistFile()
        {
            try
            {
                Playlist? newPlaylist = PlaylistFileDialog.createFile();
                if (newPlaylist != null)
                {
                    playlist = newPlaylist;
                    playlist.loadFromFile();
                    Tracks = playlist.Tracks;
                    MediaPlayMode = PlayMode.Playlist;
                    MessageBox.Show("Pomyślnie utworzono nową playliste.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                playlist = null;
                Tracks = null;
                MediaPlayMode = PlayMode.None;
                MessageBox.Show($"Błąd podczas tworzenia playlisty:\n{ex.Message}", "Błąd zapisu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void openVideo()
        {
            try
            {
                MediaElementVM.OpenVideoFile();
                MediaPlayMode = PlayMode.Video;
            }
            catch (Exception ex)
            {
                MediaPlayMode = PlayMode.None;
                MessageBox.Show($"Błąd podczas otwierania wideo:\n{ex.Message}", "Błąd zapisu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Commands
        public ICommand OpenVideoFile
        {
            get
            {
                return new RelayCommand(execute => openVideo(), canExecute => true);
            }
        }

        public ICommand CreatePlaylist
        {
            get
            {
                return new RelayCommand(execute => createPlaylistFile(), canExecute => true);
            }
        }
        #endregion
    }
}
