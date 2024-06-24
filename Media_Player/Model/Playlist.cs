using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.Model
{
    public class Playlist 
    {
        public string PlaylistName { get; private set; }
        public ObservableCollection<Song> Songs { get; private set; }

        public Playlist(string playlistname)
        {
            PlaylistName = playlistname;
            Songs = new ObservableCollection<Song>();
        }
    }
}
