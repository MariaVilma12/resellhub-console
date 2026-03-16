namespace ResellHubConsole.Models;

public class Listing
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";

    public Category Category { get; set; }

    public ItemCondition Condition { get; set; }

    public double Price { get; set; }

    public ListingStatus Status { get; set; } = ListingStatus.Available;

    public int SellerId { get; set; }
    public int? BuyerId { get; set; }
    public string SellerUsername { get; set; } = "";
}