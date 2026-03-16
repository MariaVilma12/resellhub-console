using Microsoft.Data.Sqlite;

namespace ResellHubConsole.Data
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
            if (File.Exists("marketplace.db"))
            {
                File.Delete("marketplace.db");
            }

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            CreateUsersTable(connection);
            CreateListingsTable(connection);
            CreateTransactionsTable(connection);
            CreateReviewsTable(connection);

            SeedTestData(connection);
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
        private static void SeedTestData(SqliteConnection connection)
        {
            // Create test users
            var userCommand = connection.CreateCommand();
            userCommand.CommandText =
                @"
    INSERT INTO Users (Username, Password)
    VALUES ('maria', '123'),
           ('erik', '123');
    ";
            userCommand.ExecuteNonQuery();

            // Get user IDs
            var getUsers = connection.CreateCommand();
            getUsers.CommandText = "SELECT Id, Username FROM Users;";

            var users = new Dictionary<string, int>();

            using (var reader = getUsers.ExecuteReader())
            {
                while (reader.Read())
                {
                    users[reader.GetString(1)] = reader.GetInt32(0);
                }
            }

            // Insert 5 listings for maria
            InsertListing(connection, "iPhone 14 Pro", "128GB Space Black", 0, 1, 5500, users["maria"]);
            InsertListing(connection, "MacBook Air", "M1 Chip", 0, 1, 9000, users["maria"]);
            InsertListing(connection, "Gaming Chair", "Comfortable chair", 1, 1, 1200, users["maria"]);
            InsertListing(connection, "Running Shoes", "Size 42", 2, 1, 300, users["maria"]);
            InsertListing(connection, "Desk Lamp", "LED light", 1, 1, 150, users["maria"]);

            // Insert 5 listings for erik
            InsertListing(connection, "Bike", "Mountain bike", 2, 1, 2500, users["erik"]);
            InsertListing(connection, "Tablet", "Good condition", 0, 1, 2000, users["erik"]);
            InsertListing(connection, "Books Set", "Programming books", 3, 1, 400, users["erik"]);
            InsertListing(connection, "Headphones", "Noise cancelling", 0, 1, 800, users["erik"]);
            InsertListing(connection, "Backpack", "Travel backpack", 2, 1, 350, users["erik"]);
        }
        private static void InsertListing(
            SqliteConnection connection,
            string title,
            string description,
            int category,
            int condition,
            double price,
            int sellerId)
        {
            var command = connection.CreateCommand();

            command.CommandText =
                @"
    INSERT INTO Listings
    (Title, Description, Category, Condition, Price, Status, SellerId)
    VALUES ($title, $desc, $cat, $cond, $price, 0, $seller);
    ";

            command.Parameters.AddWithValue("$title", title);
            command.Parameters.AddWithValue("$desc", description);
            command.Parameters.AddWithValue("$cat", category);
            command.Parameters.AddWithValue("$cond", condition);
            command.Parameters.AddWithValue("$price", price);
            command.Parameters.AddWithValue("$seller", sellerId);

            command.ExecuteNonQuery();
        }
    }
}