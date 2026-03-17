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
SELECT l.Id, l.Title, l.Description, l.Category, l.Condition, l.Price,
       l.Status, l.SellerId, l.BuyerId,
       u.Username
FROM Listings l
JOIN Users u ON l.SellerId = u.Id
WHERE l.Status = 0
AND l.SellerId != $currentUserId;
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
                BuyerId = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                SellerUsername = reader.GetString(9)
            });
        }

        return listings;
    }
}