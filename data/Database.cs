using Microsoft.Data.Sqlite;

public class Database
{
    private const string ConnectionString = "Data Source=marketplace.db";

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(ConnectionString);
    }
}