using Media_Player.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.ViewModel
{
    using BaseClass;
    using Media_Player.Model;
    using System.Diagnostics;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class MediaElementViewModel:ViewModelBase
    {
        public MediaElementViewModel()
        {
            _isPlaying = false;
            _volumeLevel = 0.5;
        }

        #region Properties
        private Uri? _mediaUri;
        public Uri? MediaUri
        {
            get
            { return _mediaUri; }
            set
            {
                _mediaUri = value;
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
            set {
                _isPlaying = value;
            onPropertyChanged(nameof(IsPlaying));}
        }

        private double _volumeLevel;
        public double VolumeLevel
        {
            get { return _volumeLevel; }
            set
            {
                _volumeLevel = value;
                onPropertyChanged(nameof(VolumeLevel));
            }
        }
        #endregion

        #region Methods
        public Uri? OpenMediaFile()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            //dialog.FileName = "Media"; // Default file name
            //dialog.DefaultExt = "*.*"; // Default file extension
            dialog.Filter = "avi (*.avi)|*.avi|MP4 (*.mp4)|*.mp4|All files (*.*)|*.*"; // Filter files by extension
            // Show open file dialog box
            bool? result = dialog.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                Uri uri = new Uri(filename);
                //Media = new Media(filename);
                Trace.WriteLine(uri.ToString());
                return uri;
            }
            return null;
        }

        private void playMedia()
        {
            if (PlayRequest != null)
            {
                PlayRequest(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Commands
        private ICommand? openFile = null;
        public ICommand? OpenFile
        {
            get
            {
                if (openFile == null)
                {
                    openFile = new RelayCommand(
                        execute =>
                        {
                            MediaUri = OpenMediaFile();
                        },
                        canExecute => true);
                }
                return openFile;
            }
        }

        public ICommand PlayRequested
        {
            get
            {
                return new RelayCommand(execute => playMedia(), canExecute => (MediaUri != null));
            }
        }

        #endregion
    }
}
