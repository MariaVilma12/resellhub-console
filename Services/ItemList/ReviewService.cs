using Microsoft.Data.Sqlite;
using ResellHubConsole.Data;
using ResellHubConsole.Models;

namespace ResellHubConsole.Services.ItemList;

public class ReviewService
{
    public void LeaveReview(int sellerId, int reviewerId)
    {
        Console.Write("Rating (1-6): ");
        int rating = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Comment (optional): ");
        string comment = Console.ReadLine() ?? "";

        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"INSERT INTO Reviews(SellerId, ReviewerId, Rating, Comment)
          VALUES($seller,$reviewer,$rating,$comment)";

        command.Parameters.AddWithValue("$seller", sellerId);
        command.Parameters.AddWithValue("$reviewer", reviewerId);
        command.Parameters.AddWithValue("$rating", rating);
        command.Parameters.AddWithValue("$comment", comment);

        command.ExecuteNonQuery();

        Console.WriteLine("Review saved successfully.");
    }
    public void ShowMyReviews(int sellerId)
    {
        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT Rating, Comment
          FROM Reviews
          WHERE SellerId = $seller";

        command.Parameters.AddWithValue("$seller", sellerId);

        using var reader = command.ExecuteReader();

        Console.WriteLine("\n=== My Reviews ===");

        bool found = false;
        int total = 0;
        int count = 0;

        while (reader.Read())
        {
            found = true;

            int rating = reader.GetInt32(0);
            string comment = reader.GetString(1);

            total += rating;
            count++;

            Console.WriteLine($"Rating: {rating} | Comment: {comment}");
        }

        if (!found)
        {
            Console.WriteLine("No reviews yet.");
            return;
        }

        double average = (double)total / count;
        Console.WriteLine($"\nAverage Rating: {average:F1}");
    }
}