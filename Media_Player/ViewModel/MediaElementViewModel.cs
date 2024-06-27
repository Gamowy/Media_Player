namespace Media_Player.ViewModel
{
    using BaseClass;
    using System.Diagnostics;
    using System.Windows.Input;

    public class MediaElementViewModel : ViewModelBase
    {
        public MediaElementViewModel()
        {
            _isPlaying = false;
            _volumeLevel = 0.5;
            MediaName = null;
        }

        #region Properties
        private String? _mediaName;
        public String? MediaName
        {
            get
            {
                return _mediaName;
            }
            set
            {
                if (value != null)
                {
                    _mediaName = value;
                }
                else
                {
                    _mediaName = "Aktualnie nic nie odtwarzasz";
                }
                onPropertyChanged(nameof(MediaName));
            }
        }


        private Uri? _mediaUri;
        public Uri? MediaUri
        {
            get
            { return _mediaUri; }
            set
            {
                _mediaUri = value;
                if (value != null)
                {
                    string filename = System.IO.Path.GetFileName(value.LocalPath);
                    MediaName = filename;
                }
                else
                {
                    MediaName = null;
                }
                IsPlaying = false;
                playMedia();
                onPropertyChanged(nameof(MediaUri));
            }
        }

        public event EventHandler? PlayRequest;

        private bool _isPlaying;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                onPropertyChanged(nameof(IsPlaying));
            }
        }

        public event EventHandler? VolumeButtonUpdate;

        private double _previousVolumeLevel;

        private double _volumeLevel;
        public double VolumeLevel
        {
            get { return _volumeLevel; }
            set
            {
                _previousVolumeLevel = _volumeLevel;
                _volumeLevel = value;
                onPropertyChanged(nameof(VolumeLevel));
                if (VolumeButtonUpdate != null)
                {
                    VolumeButtonUpdate(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Methods
        public void OpenVideoFile()
        {
            try
            {
                // Configure open file dialog box
                var dialog = new Microsoft.Win32.OpenFileDialog();
                //dialog.FileName = "Media"; // Default file name
                //dialog.DefaultExt = "*.*"; // Default file extension
                dialog.Filter = "Media (*.avi, *.mp4)|*.avi;*.mp4|All files (*.*)|*.*"; // Filter files by extension
                                                                                        // Show open file dialog box
                bool? result = dialog.ShowDialog();
                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = dialog.FileName;
                    Uri uri = new Uri(filename);
                    Trace.WriteLine(uri.ToString());
                    MediaUri = uri;
                }
                else
                {
                    MediaUri = null;
                }
            }
            catch (Exception ex)
            {
                MediaUri = null;
                throw new Exception(ex.Message);
            }
        }

        private void playMedia()
        {
            if (PlayRequest != null)
            {
                PlayRequest(this, EventArgs.Empty);
            }
        }

        private void muteAudio()
        {
            if (VolumeLevel == 0)
            {
                VolumeLevel = _previousVolumeLevel;
            }
            else
            {
                VolumeLevel = 0;
            }
        }


        #endregion

        #region Commands
        public ICommand PlayRequested
        {
            get
            {
                return new RelayCommand(execute => playMedia(), canExecute => (MediaUri != null));
            }
        }

        public ICommand MuteAudio
        {
            get
            {
                return new RelayCommand(execute => muteAudio(), canExecute => true);
            }
        }

        #endregion
    }
}
