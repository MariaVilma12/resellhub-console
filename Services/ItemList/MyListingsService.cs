using Microsoft.Data.Sqlite;
using ResellHubConsole.Data;

namespace ResellHubConsole.Services.ItemList;

public class MyListingsService
{
    public void ShowMyListings(int sellerId)
    {
        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT Title, Price, Status
          FROM Listings
          WHERE SellerId = $id";

        command.Parameters.AddWithValue("$id", sellerId);

        using var reader = command.ExecuteReader();

        Console.WriteLine("\n=== My Listings ===");

        bool found = false;

        while (reader.Read())
        {
            found = true;

            string title = reader.GetString(0);
            double price = reader.GetDouble(1);
            string status = reader.GetString(2);

            Console.WriteLine($"{title} | {price} kr | {status}");
        }

        if (!found)
            Console.WriteLine("You have no listings.");
    }
}