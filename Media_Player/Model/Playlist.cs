using QuizApp.Model.Database;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Formats.Tar;
using System.IO;
using System.Windows;

namespace Media_Player.Model
{
    public class Playlist
    {
        #region SQL COMMANDS
        private const string CREATE_TRACKS_TB = "CREATE TABLE tracks (id int primary key, track_name text not null, artist text, album text, genre text, release_year int, file_path text)";
        private const string SELECT_TRACKS = "SELECT * from tracks";
        private const string DELETE_TRACKS = "DELETE FROM tracks";
        private const string INSERT_TRACKS = "INSERT INTO tracks VALUES (@track_id, @track_name, @artist, @album, @genre, @release_year, @file_path)";
        #endregion

        public string Name { get; private set; }
        public string Path { get; private set; }
        public int LastTrackId { get; private set; }
        public ObservableCollection<Track> Tracks { get; private set; }

        public Playlist(string playlistname, string playlistPath)
        {
            Name = playlistname;
            Path = playlistPath;
            LastTrackId = -1;
            Tracks = new ObservableCollection<Track>();
        }

        public void create()
        {
            try
            {
                if (!File.Exists(Path))
                {
                    File.Delete(Path);
                }
                SQLiteConnection.CreateFile(Path);
                SQLiteConnection connection = DbConnection.Instance.getConnection(Path);
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
                SQLiteConnection connection = DbConnection.Instance.getConnection(Path);
                connection.Open();

                // read questions from database
                SQLiteCommand selectCommand = new SQLiteCommand(SELECT_TRACKS, connection);
                SQLiteDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["id"];
                    string trackName = reader["track_name"].ToString()!;
                    string? artist = reader["artist"].ToString();
                    string? album = reader["album"].ToString();
                    string? genre = reader["genre"].ToString();
                    int? releaseYear = null;
                    if(int.TryParse(reader["release_year"].ToString(), out int result))
                    {
                        releaseYear = (int)reader["release_year"];
                    }
                    string filePath = reader["file_path"].ToString()!;

                    if (id > LastTrackId)
                    {
                        LastTrackId = id;
                    }

                    Track track = new Track(id, trackName, filePath);
                    track.TrackName = trackName;
                    track.Artist = artist;
                    track.Album = album;
                    track.Genre = genre;
                    track.ReleaseYear = releaseYear;

                    try
                    {
                        var tfile = TagLib.File.Create(track.FilePath);
                        if (tfile.Tag.Pictures.Count() > 0)
                        {
                            track.CoverImage = tfile.Tag.Pictures[0];
                        }
                    } catch (Exception ex)
                    {
                        MessageBox.Show($"Lokalizacja niektórych plików uległa zmianie:\n{ex.Message}", "Zmiana lokalizacji utworów!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

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
            track.Id = ++LastTrackId;
            Tracks.Add(track);
        }

        public void removeTrack(int? removeTrackId)
        {
            if (removeTrackId != null)
            {
                foreach (Track track in Tracks.ToList())
                {
                    if (track.Id == removeTrackId)
                    {
                        Tracks.Remove(track);
                    }
                }
            }
        }

        public void save()
        {
            try
            {
                SQLiteConnection connection = DbConnection.Instance.getConnection(Path);
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
                    insertNew.Parameters.AddWithValue("file_path", track.FilePath);
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
