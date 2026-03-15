using ResellHubConsole.Models;
using ResellHubConsole.Services.ItemList;

namespace ResellHubConsole.Services.Menus;

public class MainMenu
{
    private readonly ListingService _listingService = new();

    public void Show(User user)
    {
        while (true)
        {
            Console.WriteLine("\n=== Main Menu ===");
            Console.WriteLine("1. Create Listing");
            Console.WriteLine("2. Browse Listings");
            Console.WriteLine("3. Search Listings");
            Console.WriteLine("4. My Listings");
            Console.WriteLine("5. My Purchases");
            Console.WriteLine("6. My Reviews");
            Console.WriteLine("7. Log Out");

            Console.Write("Select option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "2":
                    BrowseListings();
                    break;

                case "7":
                    return;

                default:
                    Console.WriteLine("Feature not implemented yet.");
                    break;
            }
        }
    }

    private void BrowseListings()
    {
        var listings = _listingService.GetAvailableListings();

        Console.WriteLine("\n=== Available Listings ===");

        Console.WriteLine($"{"#",3} {"Title",-20} {"Category",-15} {"Condition",-10} {"Price"}");

        int index = 1;

        foreach (var item in listings)
        {
            Console.WriteLine($"{index,3} {item.Title,-20} {item.Category,-15} {item.Condition,-10} {item.Price} kr");
            index++;
        }

        Console.Write("Select listing (0 to go back): ");
        int selected = int.Parse(Console.ReadLine() ?? "0");

        if (selected <= 0 || selected > listings.Count)
            return;

        var listing = listings[selected - 1];

        ShowListingDetails(listing);
    }

    private void ShowListingDetails(Listing listing)
    {
        Console.WriteLine($"\n=== {listing.Title} ===");
        Console.WriteLine($"Seller: {listing.SellerUsername}");
        Console.WriteLine($"Category: {listing.Category}");
        Console.WriteLine($"Condition: {listing.Condition}");
        Console.WriteLine($"Price: {listing.Price} kr");
        Console.WriteLine($"Description: {listing.Description}");

        Console.WriteLine("\n1. Buy this item");
        Console.WriteLine("2. Go back");

        Console.ReadLine();
    }
}