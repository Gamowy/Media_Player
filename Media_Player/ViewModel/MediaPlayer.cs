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
    using System.Windows.Markup.Localizer;

    public enum PlayMode { None, Video, Playlist};

    public class MediaPlayer : ViewModelBase
    {
        public MediaPlayer()
        {
            MediaElementVM = new MediaElementViewModel();
            addTrackWindow = null;
            playlist = null;
            MediaPlayMode = PlayMode.None;
        }

        #region Properties

        AddTrackWindow? addTrackWindow;

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
                switch (value) {
                    case PlayMode.None:
                        MediaElementVM.MediaUri = null;
                        playlist = null;
                        Tracks = null;
                        SelectedTrack = null;
                        PlaylistName = "Playlista";
                        break;
                    case PlayMode.Video:
                        playlist = null;
                        Tracks = null;
                        SelectedTrack = null;
                        PlaylistName = "Playlista";
                        break;
                    case PlayMode.Playlist:
                        MediaElementVM.MediaUri = null;
                        PlaylistName = playlist!.Name;
                        break;
                }
                onPropertyChanged(nameof(PlayMode));
            }
        }


        private string? playlistName;
        public string? PlaylistName
        {
            get
            {
                return playlistName;
            }
            set
            {
                playlistName = value;
                onPropertyChanged(nameof(PlaylistName));
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
                PlaylistName = null;
                Tracks = null;
                MediaPlayMode = PlayMode.None;
                MessageBox.Show($"Błąd podczas tworzenia playlisty:\n{ex.Message}", "Błąd zapisu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void openPlaylistFile()
        {
            try
            {
                Playlist? newPlaylist = PlaylistFileDialog.openFile();
                if (newPlaylist != null)
                {
                    playlist = newPlaylist;
                    playlist.loadFromFile();
                    Tracks = playlist.Tracks;
                    MediaPlayMode = PlayMode.Playlist;
                    MessageBox.Show("Pomyślnie otworzno playliste.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                playlist = null;
                PlaylistName = null;
                Tracks = null;
                MediaPlayMode = PlayMode.None;
                MessageBox.Show($"Błąd podczas otwierania playlisty:\n{ex.Message}", "Błąd odczytu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void savePlaylist()
        {
            try
            {
                playlist!.save();
                MessageBox.Show("Pomyślnie zapisano playliste.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania playlisty:\n{ex.Message}", "Błąd zapisu!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void openAddTrackWindow()
        {
            ViewModelShare.playlistShare = playlist;
            addTrackWindow = new AddTrackWindow();
            addTrackWindow.ShowDialog();
            Tracks = playlist?.Tracks;
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

        public ICommand OpenPlaylist
        {
            get
            {
                return new RelayCommand(execute => openPlaylistFile(), canExecute => true);
            }
        }

        public ICommand SavePlaylist
        {
            get
            {
                return new RelayCommand(execute => savePlaylist(), canExecute => (playlist != null));
            }
        }

        public ICommand AddNewTrack
        {
            get
            {
                return new RelayCommand(execute => openAddTrackWindow(), canExecute => (playlist != null));
            }
        }
        #endregion
    }
}
