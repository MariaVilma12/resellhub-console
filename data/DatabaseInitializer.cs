using Microsoft.Data.Sqlite;

namespace MarketplaceApp.Data
{
    /// <summary>
    /// Responsible for creating and initializing the SQLite database.
    /// </summary>
    public static class DatabaseInitializer
    {
        private const string ConnectionString = "Data Source=marketplace.db";

        /// <summary>
        /// Initializes the database and creates tables if they do not exist.
        /// </summary>
        public static void Initialize()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            CreateUsersTable(connection);
            CreateListingsTable(connection);
            CreateTransactionsTable(connection);
            CreateReviewsTable(connection);
        }

        private static void CreateUsersTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL
            );
            ";

            command.ExecuteNonQuery();
        }

        private static void CreateListingsTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Listings (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT,
                Category INTEGER NOT NULL,
                Condition INTEGER NOT NULL,
                Price REAL NOT NULL,
                Status INTEGER NOT NULL,
                SellerId INTEGER NOT NULL,
                BuyerId INTEGER,
                FOREIGN KEY (SellerId) REFERENCES Users(Id),
                FOREIGN KEY (BuyerId) REFERENCES Users(Id)
            );
            ";

            command.ExecuteNonQuery();
        }

        private static void CreateTransactionsTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Transactions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ListingId INTEGER NOT NULL,
                BuyerId INTEGER NOT NULL,
                SellerId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                FOREIGN KEY (ListingId) REFERENCES Listings(Id),
                FOREIGN KEY (BuyerId) REFERENCES Users(Id),
                FOREIGN KEY (SellerId) REFERENCES Users(Id)
            );
            ";

            command.ExecuteNonQuery();
        }

        private static void CreateReviewsTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Reviews (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TransactionId INTEGER NOT NULL,
                ReviewerId INTEGER NOT NULL,
                SellerId INTEGER NOT NULL,
                Rating INTEGER NOT NULL,
                Comment TEXT,
                FOREIGN KEY (TransactionId) REFERENCES Transactions(Id),
                FOREIGN KEY (ReviewerId) REFERENCES Users(Id),
                FOREIGN KEY (SellerId) REFERENCES Users(Id)
            );
            ";

            command.ExecuteNonQuery();
        }
    }
}