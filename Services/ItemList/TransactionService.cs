using Microsoft.Data.Sqlite;
using ResellHubConsole.Data;

namespace ResellHubConsole.Services.ItemList;

public class TransactionService
{
    public void ShowMySales(int userId)
    {
        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
        SELECT l.Title, l.Price, t.Date, u.Username
        FROM Transactions t
        JOIN Listings l ON t.ListingId = l.Id
        JOIN Users u ON t.BuyerId = u.Id
        WHERE t.SellerId = $user;
        ";

        command.Parameters.AddWithValue("$user", userId);

        using var reader = command.ExecuteReader();

        Console.WriteLine("\n=== My Sales ===");

        if (!reader.HasRows)
        {
            Console.WriteLine("No items sold yet.");
            return;
        }

        while (reader.Read())
        {
            Console.WriteLine($"\nItem: {reader.GetString(0)}");
            Console.WriteLine($"Price: {reader.GetDouble(1)} kr");
            Console.WriteLine($"Date: {reader.GetString(2)}");
            Console.WriteLine($"Buyer: {reader.GetString(3)}");
        }
    }

    public void ShowMyPurchases(int userId)
    {
        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
        SELECT l.Title, l.Price, t.Date, u.Username
        FROM Transactions t
        JOIN Listings l ON t.ListingId = l.Id
        JOIN Users u ON t.SellerId = u.Id
        WHERE t.BuyerId = $user;
        ";

        command.Parameters.AddWithValue("$user", userId);

        using var reader = command.ExecuteReader();

        Console.WriteLine("\n=== My Purchases ===");

        if (!reader.HasRows)
        {
            Console.WriteLine("No purchases yet.");
            return;
        }

        while (reader.Read())
        {
            Console.WriteLine($"\nItem: {reader.GetString(0)}");
            Console.WriteLine($"Price: {reader.GetDouble(1)} kr");
            Console.WriteLine($"Date: {reader.GetString(2)}");
            Console.WriteLine($"Seller: {reader.GetString(3)}");
        }
    }
}