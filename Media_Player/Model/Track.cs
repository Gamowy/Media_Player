﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.Model
{
    public class Track
    {
        public int Id { get; set; }
        public string TrackName { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public double? Duration { get; set; }
        public Uri FilePath { get; set; }

        public Track(int id, string trackname, Uri filepath)
        {
            Id = id;
            TrackName = trackname;
            FilePath = filepath;
        }
    }
}