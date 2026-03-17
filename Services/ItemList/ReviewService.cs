using Microsoft.Data.Sqlite;
using ResellHubConsole.Data;
using ResellHubConsole.Models;

namespace ResellHubConsole.Services.ItemList;

public class ReviewService
{
    public void LeaveReview(int transactionId, int reviewerId, int sellerId)
    {
        Console.Write("\nRating (1-6): ");
        int rating = int.Parse(Console.ReadLine() ?? "1");

        Console.Write("Comment (or press Enter to skip): ");
        string? comment = Console.ReadLine();

        using var connection = Database.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
    INSERT INTO Reviews (TransactionId, ReviewerId, SellerId, Rating, Comment)
    VALUES ($transactionId, $reviewerId, $sellerId, $rating, $comment);
    ";

        command.Parameters.AddWithValue("$transactionId", transactionId);
        command.Parameters.AddWithValue("$reviewerId", reviewerId);
        command.Parameters.AddWithValue("$sellerId", sellerId);
        command.Parameters.AddWithValue("$rating", rating);
        command.Parameters.AddWithValue("$comment", comment ?? "");

        command.ExecuteNonQuery();

        Console.WriteLine("Review submitted. Thank you!");
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