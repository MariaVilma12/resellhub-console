using Microsoft.Data.Sqlite;
using ResellHubConsole.Models;
using ResellHubConsole.Data;
using ResellHubConsole.Utils;

namespace ResellHubConsole.Services.ItemList;

public class CreateListingService
{
    public void CreateListing(User user)
    {
        Console.WriteLine("\n=== Create Listing ===");

        string title = InputHelper.ReadRequiredString("Title: ");
        string description = InputHelper.ReadRequiredString("Description: ");
        Category category = InputHelper.ReadEnum<Category>("Choose Category:");
        ItemCondition condition = InputHelper.ReadEnum<ItemCondition>("Choose Condition:");
        double price = InputHelper.ReadPositiveDouble("Price: ");

        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"INSERT INTO Listings
        (Title,Description,Category,Condition,Price,Status,SellerId)
        VALUES($title,$desc,$cat,$cond,$price,'Available',$seller)";

        command.Parameters.AddWithValue("$title", title);
        command.Parameters.AddWithValue("$desc", description);
        command.Parameters.AddWithValue("$cat", category);
        command.Parameters.AddWithValue("$cond", condition);
        command.Parameters.AddWithValue("$price", price);
        command.Parameters.AddWithValue("$seller", user.Id);

        command.ExecuteNonQuery();

        Console.WriteLine("Listing created successfully.");
    }
}