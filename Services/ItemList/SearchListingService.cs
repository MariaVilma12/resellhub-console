using ResellHubConsole.Models;

namespace ResellHubConsole.Services.ItemList;

public class SearchListingService
{
    private readonly ListingService _listingService = new();

    public void SearchListings(int currentUserId)
    {
        Console.Write("\nEnter keyword: ");
        string keyword = Console.ReadLine()!.ToLower();

        var listings = _listingService.GetAvailableListings(currentUserId);

        var results = listings
            .Where(l => l.Title.ToLower().Contains(keyword)
                        || l.Description.ToLower().Contains(keyword))
            .ToList();

        Console.WriteLine("\n=== Search Results ===");

        if (!results.Any())
        {
            Console.WriteLine("No listings found.");
            return;
        }

        foreach (var item in results)
        {
            Console.WriteLine($"{item.Title} | {item.Price} kr");
        }
    }
}