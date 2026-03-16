using Microsoft.Data.Sqlite;
using ResellHubConsole.Models;
using ResellHubConsole.Data;

namespace ResellHubConsole.Services.ItemList;

public class ListingService
{
    public List<Listing> GetAvailableListings(int currentUserId)
    {
        var listings = new List<Listing>();

        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"
    SELECT Id, Title, Description, Category, Condition, Price, Status, SellerId, BuyerId
    FROM Listings
    WHERE Status = 0
    AND SellerId != $currentUserId;
    ";

        command.Parameters.AddWithValue("$currentUserId", currentUserId);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            listings.Add(new Listing
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Category = (Category)reader.GetInt32(3),
                Condition = (ItemCondition)reader.GetInt32(4),
                Price = reader.GetDouble(5),
                Status = (ListingStatus)reader.GetInt32(6),
                SellerId = reader.GetInt32(7),
                BuyerId = reader.IsDBNull(8) ? null : reader.GetInt32(8)
            });
        }

        return listings;
    }

    public void DisplayListings(User currentUser)
    {
        var listings = GetAvailableListings(currentUser.Id);

        Console.WriteLine("\n=== Available Listings ===");

        Console.WriteLine($"{"#",3} {"Title",-20} {"Category",-15} {"Condition",-10} {"Price"}");

        int index = 1;

        foreach (var item in listings)
        {
            Console.WriteLine($"{index,3} {item.Title,-20} {item.Category,-15} {item.Condition,-10} {item.Price} kr");
            index++;
        }

        Console.Write("\nSelect a listing (0 to go back): ");

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

        Console.WriteLine("\n1. Buy this item");
        Console.WriteLine("2. Go back");

        var option = Console.ReadLine();

        if (option == "1")
        {
            var purchaseService = new PurchaseService();
            purchaseService.BuyItem(selected, currentUser);

            Console.Write("\nLeave review? (Y/N): ");
            var answer = Console.ReadLine();

            if (answer?.ToUpper() == "Y")
            {
                var reviewService = new ReviewService();
                reviewService.LeaveReview(selected.SellerId, currentUser.Id);
            }
        }
    }
}