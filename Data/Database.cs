using Microsoft.Data.Sqlite;

namespace ResellHubConsole.Data
{
    public class Database
    {
        private const string ConnectionString = "Data Source=marketplace.db";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}