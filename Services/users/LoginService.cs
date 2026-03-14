using Microsoft.Data.Sqlite;
using ResellHubConsole.Models;
using ResellHubConsole.Data; 

namespace ResellHubConsole.Services.Users
{
    public class LoginService
    {
        public User? Login(string username, string password)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT Id, Username, Password
              FROM Users
              WHERE Username = $username
              AND Password = $password;";

            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$password", password);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                };
            }

            return null;
        }
    }
}