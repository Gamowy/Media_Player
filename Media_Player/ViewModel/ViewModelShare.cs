using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.ViewModel
{
    using Model;
    using System.Collections.ObjectModel;

    public static class ViewModelShare
    {
        public static Playlist? playlistShare = null;
        public static Track? selectedTrackShare = null;
    }
}
