namespace ResellHubConsole.Models;

public class Review
{
    public int Id { get; set; }

    public int SellerId { get; set; }

    public int ReviewerId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = "";
}