namespace Media_Player.Model
{
    public static class PlaylistFileDialog
    {
        public static Playlist? createFile()
        {
            string filePath = string.Empty;

            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "playlist files (*.playlist)|*.playlist|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                if (filePath != string.Empty)
                {
                    Playlist playlist = new Playlist(System.IO.Path.GetFileNameWithoutExtension(filePath), filePath);
                    playlist.create();
                    return playlist;
                }
            }
            return null;
        }
        public static Playlist? openFile()
        {
            string filePath = string.Empty;

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "playlist files (*.playlist)|*.playlist|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                if (filePath != string.Empty)
                {
                    return new Playlist(System.IO.Path.GetFileNameWithoutExtension(filePath), filePath);
                }
            }
            return null;
        }
    }
}
