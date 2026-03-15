using Microsoft.Data.Sqlite;
using ResellHubConsole.Models;
using ResellHubConsole.Data;

namespace ResellHubConsole.Services.ItemList;

public class ListingService
{
    public List<Listing> GetAvailableListings()
    {
        var listings = new List<Listing>();

        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            @"SELECT l.Id, l.Title, l.Description, l.Category, l.Condition, l.Price,
          u.Username
          FROM Listings l
          JOIN Users u ON l.SellerId = u.Id
          WHERE l.Status = 'Available';";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var listing = new Listing
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                Category = Enum.Parse<Category>(reader.GetString(3)),
                Condition = Enum.Parse<ItemCondition>(reader.GetString(4)),
                Price = reader.GetDouble(5),
                SellerUsername = reader.GetString(6)
            };

            listings.Add(listing);
        }

        return listings;
    }
    
    public void DisplayListings()
    {
        var listings = GetAvailableListings();

        Console.WriteLine("\n=== Available Listings ===");

        Console.WriteLine($"{"#",3} {"Title",-20} {"Category",-15} {"Condition",-10} {"Price"}");

        int index = 1;

        foreach (var item in listings)
        {
            Console.WriteLine($"{index,3} {item.Title,-20} {item.Category,-15} {item.Condition,-10} {item.Price} kr");
            index++;
        }

        Console.WriteLine("\nSelect a listing (0 to go back):");

        int choice = int.Parse(Console.ReadLine() ?? "0");

        if (choice == 0 || choice > listings.Count)
            return;

        var selected = listings[choice - 1];

        Console.WriteLine($"\n=== {selected.Title} ===");
        Console.WriteLine($"Seller: {selected.SellerUsername}");
        Console.WriteLine($"Category: {selected.Category}");
        Console.WriteLine($"Condition: {selected.Condition}");
        Console.WriteLine($"Price: {selected.Price} kr");
        Console.WriteLine($"Description: {selected.Description}");
    }
}