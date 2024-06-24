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
        private Model model;

        public Playlist? playlist;
        public MediaElementViewModel MediaElementVM { get; set; }

        public MediaPlayer()
        {
            model=new Model();
            MediaElementVM = new MediaElementViewModel();
        }
    }
}
