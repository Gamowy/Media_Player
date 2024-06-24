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
    using System.Windows.Input;

    public class MediaElementViewModel:ViewModelBase
    {
        MediaControl MediaControl;
        public MediaElementViewModel()
        {
            MediaControl=new MediaControl();
        }

        #region Properties
        public Uri MediaUri {
            get
            { return MediaControl.MediaUri; }
            private set
            {  MediaControl.MediaUri = value;
                onPropertyChanged(nameof(MediaControl.MediaUri));
            }
            }

        public bool IsPlaying
        {
            get { return MediaControl.IsPlaying; }
            set { MediaControl.IsPlaying = value;
            onPropertyChanged(nameof(MediaControl.IsPlaying));}
        }
        #endregion

        #region Methods
        public Uri OpenMediaFile()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Media"; // Default file name
                                       //dialog.DefaultExt = ".txt"; // Default file extension
                                       // dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                Trace.WriteLine(filename);
                Uri uri = new Uri(filename);
                Trace.WriteLine(uri.ToString());
                return uri;
            }
            return null;
        }
        #endregion

        #region Commands
        private ICommand openFile = null;
        public ICommand OpenFile
        {
            get
            {
                if (openFile == null)
                {
                    openFile = new RelayCommand(
                        arg =>
                        {
                            MediaUri = OpenMediaFile();
                        },
                        arg => true);
                }
                return openFile;
            }
        }
        #endregion
    }
}
