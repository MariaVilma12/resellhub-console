using ResellHubConsole.Models;
using ResellHubConsole.Services.ItemList;
using ResellHubConsole.Utils;

namespace ResellHubConsole.Services.Menus;

public class MainMenu
{
    private readonly ListingService _listingService = new();
    private readonly CreateListingService _createService = new();
    private readonly SearchListingService _searchService = new();
    private readonly MyListingsService _myListingsService = new();
    private readonly PurchaseService _purchaseService = new();
    private readonly ReviewService _reviewService = new();
    private readonly TransactionService _transactionService = new();

    public void Show(User user)
    {
        while (true)
        {
            Console.WriteLine("\n=== Main Menu ===");
            Console.WriteLine("1. Create Listing");
            Console.WriteLine("2. Browse Listings");
            Console.WriteLine("3. Search Listings");
            Console.WriteLine("4. My Listings");
            Console.WriteLine("5. My Sales");
            Console.WriteLine("6. My Purchases");
            Console.WriteLine("7. My Reviews");
            Console.WriteLine("8. Log Out");

            Console.Write("Select option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _createService.CreateListing(user);
                    break;

                case "2":
                    BrowseListings(user);
                    break;

                case "3":
                    _searchService.SearchListings(user.Id);
                    break;

                case "4":
                    _myListingsService.ShowMyListings(user.Id);
                    break;
				case "5":
                   _transactionService.ShowMySales(user.Id);
                    break;

               case "6":
                   _transactionService.ShowMyPurchases(user.Id);
                    break;

                case "7":
                    _reviewService.ShowMyReviews(user.Id);
                    break;

                case "8":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private void BrowseListings(User user)
    {
        var listings = _listingService.GetAvailableListings(user.Id);

        if (listings.Count == 0)
        {
            Console.WriteLine("No listings available.");
            return;
        }

        Console.WriteLine("\n=== Available Listings ===");
        Console.WriteLine($"{"#",3} {"Title",-20} {"Category",-15} {"Condition",-10} {"Price"}");

        int index = 1;

        foreach (var item in listings)
        {
            Console.WriteLine($"{index,3} {item.Title,-20} {item.Category,-15} {item.Condition,-10} {item.Price} kr");
            index++;
        }

        int selected = InputHelper.ReadInt("\nSelect listing (0 to go back): ");

        if (selected <= 0 || selected > listings.Count)
            return;

        var listing = listings[selected - 1];

        ShowListingDetails(listing, user);
    }

    private void ShowListingDetails(Listing listing, User user)
    {
        Console.WriteLine($"\n=== {listing.Title} ===");
        Console.WriteLine($"Seller: {listing.SellerUsername}");
        Console.WriteLine($"Category: {listing.Category}");
        Console.WriteLine($"Condition: {listing.Condition}");
        Console.WriteLine($"Price: {listing.Price} kr");
        Console.WriteLine($"Description: {listing.Description}");

        Console.WriteLine("\n1. Buy this item");
        Console.WriteLine("2. Go back");

        var option = Console.ReadLine();

        if (option == "1")
        {
            int transactionId = _purchaseService.BuyItem(listing, user);
            if (transactionId == -1)
                return;
            // Ask review BEFORE leaving method
            Console.Write($"\nWould you like to leave a review for {listing.SellerUsername}? (Y/N): ");
            var answer = Console.ReadLine();

            if (answer?.Trim().ToUpper() == "Y")
            {
                var reviewService = new ReviewService();
                reviewService.LeaveReview(transactionId, user.Id, listing.SellerId);
            }

            // Optional pause so user can see output
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}