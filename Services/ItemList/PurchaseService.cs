using Microsoft.Data.Sqlite;
using ResellHubConsole.Data;
using ResellHubConsole.Models;

namespace ResellHubConsole.Services.ItemList;

public class PurchaseService
{
    public void BuyItem(Listing listing, User buyer)
    {
        if (listing.SellerId == buyer.Id)
        {
            Console.WriteLine("You cannot buy your own item.");
            return;
        }

        if (listing.Status == ListingStatus.Sold)
        {
            Console.WriteLine("This item has already been sold.");
            return;
        }

        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"INSERT INTO Transactions(ListingId,BuyerId,SellerId,PurchaseDate)
      VALUES($listing,$buyer,$seller,$date)";

        command.Parameters.AddWithValue("$listing", listing.Id);
        command.Parameters.AddWithValue("$buyer", buyer.Id);
        command.Parameters.AddWithValue("$seller", listing.SellerId);
        command.Parameters.AddWithValue("$date", DateTime.Now.ToString());

        command.ExecuteNonQuery();

        var update = connection.CreateCommand();

        update.CommandText =
            @"UPDATE Listings
      SET Status='Sold'
      WHERE Id=$id";

        update.Parameters.AddWithValue("$id", listing.Id);

        update.ExecuteNonQuery();

        Console.WriteLine($"\nPurchase complete! You bought \"{listing.Title}\".");
    }
    public void ShowMyPurchases(int buyerId)
    {
        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT l.Title, l.Price, t.PurchaseDate
          FROM Transactions t
          JOIN Listings l ON t.ListingId = l.Id
          WHERE t.BuyerId = $buyer";

        command.Parameters.AddWithValue("$buyer", buyerId);

        using var reader = command.ExecuteReader();

        Console.WriteLine("\n=== My Purchases ===");

        bool found = false;

        while (reader.Read())
        {
            found = true;

            string title = reader.GetString(0);
            double price = reader.GetDouble(1);
            string date = reader.GetString(2);

            Console.WriteLine($"{title} | {price} kr | Purchased on: {date}");
        }

        if (!found)
            Console.WriteLine("You have not purchased anything yet.");
    }
}