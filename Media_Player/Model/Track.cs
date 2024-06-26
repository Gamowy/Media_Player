using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace Media_Player.Model
{
    public class Track
    {
        public int? Id { get; set; }
        public string TrackName { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public string FilePath { get; set; }
        public IPicture? CoverImage { get; set; }

        public Track(int? id, string trackname, string filepath)
        {
            Id = id;
            TrackName = trackname;
            FilePath = filepath;
        }
    }
}
