namespace BookApp.Domain.Entities;

public class PriceOffer
{
    public long Id { get; private set; }
    public decimal NewPrice { get; set; }
    public string? PromotionalText { get; set; }
    public long BookId { get; set; }
}