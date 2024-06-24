using System.Data.SQLite;

namespace QuizApp.Model.Database
{
    public class DbConnection
    {
        private static DbConnection? _instance = null;
        public static DbConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DbConnection();
                return _instance;
            }
        }

        private SQLiteConnection? _connection = null;
        public SQLiteConnection getConnection(string databasePath)
        {
            if (_connection != null)
                _connection.Dispose();
            _connection = new SQLiteConnection($"Data Source={databasePath}");
            return _connection;
        }

        private DbConnection() { }
    }
}
