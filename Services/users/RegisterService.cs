using Microsoft.Data.Sqlite;
using ResellHubConsole.Models;
using ResellHubConsole.Data; 
namespace ResellHubConsole.Services.Users
{
    public class RegisterService
    {
        public void Register(string username, string password)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO Users (Username, Password)
              VALUES ($username, $password);";

            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$password", password);

            command.ExecuteNonQuery();
        }
    }
}