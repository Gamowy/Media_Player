using QuizApp.Model.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player.Model
{
    public class Playlist 
    {
        #region SQL COMMANDS
        private const string CREATE_TRACKS_TB = "CREATE TABLE tracks (id int primary key, track_name text not null, artist text, album text, genre text, release_year int, duration real, file_path text)";
        private const string SELECT_TRACKS = "SELECT * from tracks";
        private const string DELETE_TRACKS = "DELETE FROM tracks";
        private const string INSERT_TRACKS = "INSERT INTO tracks VALUES (@track_id, @track_name, @artist, @album, @genre, @release_year, @duration, @file_path)";
        #endregion

        public string PlaylistName { get; private set; }
        public string PlaylistPath { get; private set; }
        public ObservableCollection<Track> Tracks { get; private set; }

        public Playlist(string playlistname, string playlistPath)
        {
            PlaylistName = playlistname;
            PlaylistPath = playlistPath;
            Tracks = new ObservableCollection<Track>();
        }

        public void create()
        {
            try
            {
                if (!File.Exists(PlaylistPath))
                {
                    File.Delete(PlaylistPath);
                }
                SQLiteConnection.CreateFile(PlaylistPath);
                SQLiteConnection connection = DbConnection.Instance.getConnection(PlaylistPath);
                connection.Open();
                // create tracks table
                SQLiteCommand createCommand = new SQLiteCommand(CREATE_TRACKS_TB, connection);
                createCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void loadFromFile()
        {
            try
            {
                SQLiteConnection connection = DbConnection.Instance.getConnection(PlaylistPath);
                connection.Open();

                // read questions from database
                SQLiteCommand selectCommand = new SQLiteCommand(SELECT_TRACKS, connection);
                SQLiteDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["id"];
                    string trackName = reader["name"].ToString()!;
                    string? artist = reader["artist"].ToString();
                    string? album = reader["album"].ToString();
                    string? genre = reader["genre"].ToString();
                    int? releaseYear = (int)reader["release_year"];
                    double? duration = (double)reader["duration"];
                    string filePath = reader["file_path"].ToString()!;

     
                    Uri fileUri = new Uri(filePath, UriKind.Absolute);
                    Track track = new Track(id, trackName, fileUri);
                    Tracks.Add(track);
                }
                connection.Clone();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void addTrack(Track track)
        {
            Tracks.Add(track);
        }

        public void removeTrack(int removeTrackId)
        {
            foreach (Track track in Tracks)
            {
                if (track.Id == removeTrackId)
                {
                    Tracks.Remove(track);
                }
            }
        }

        public void save()
        {
            try
            {
                SQLiteConnection connection = DbConnection.Instance.getConnection(PlaylistPath);
                connection.Open();

                // delete old track and insert updated tracks
                SQLiteCommand deleteOld = new SQLiteCommand(DELETE_TRACKS, connection);
                SQLiteCommand insertNew = new SQLiteCommand(INSERT_TRACKS, connection);
                deleteOld.ExecuteNonQuery();
                foreach (Track track in Tracks)
                {
                    insertNew.Parameters.AddWithValue("track_id", track.Id);
                    insertNew.Parameters.AddWithValue("track_name", track.TrackName);
                    insertNew.Parameters.AddWithValue("artist", track.Artist);
                    insertNew.Parameters.AddWithValue("album", track.Album);
                    insertNew.Parameters.AddWithValue("genre", track.Genre);
                    insertNew.Parameters.AddWithValue("release_year", track.ReleaseYear);
                    insertNew.Parameters.AddWithValue("duration", track.Duration);
                    insertNew.Parameters.AddWithValue("file_path", track.FilePath.AbsolutePath);
                    insertNew.ExecuteNonQuery();
                }
                connection.Clone();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
