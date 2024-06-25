using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.Model
{
    public class Track
    {
        int Id {  get; set; }
        string Name { get; set; }
        string? Artist { get; set; }
        string? Album { get; set; }
        string? Genre { get; set; }
        int? ReleaseYear { get; set; }
        int? Duration { get; set; }
        Uri FilePath { get; set; }

        public Track(int id, string name, Uri filepath)
        {
            Id = id;
            Name = name;
            FilePath = filepath;
        }
    }
}
