namespace ResellHubConsole.Models;

public class Transaction
{
    public int Id { get; set; }

    public int ListingId { get; set; }

    public int BuyerId { get; set; }

    public int SellerId { get; set; }

    public DateTime PurchaseDate { get; set; }
}