using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.ViewModel
{
    using BaseClass;
    using Media_Player.Model;

    public class MediaPlayer : ViewModelBase
    {
        public Playlist? playlist;
        public MediaElementViewModel MediaElementVM { get; set; }

        public MediaPlayer()
        {
            MediaElementVM = new MediaElementViewModel();
        }
    }
}
