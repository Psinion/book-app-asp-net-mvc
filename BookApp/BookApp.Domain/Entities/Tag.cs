namespace BookApp.Domain.Entities;

public class Tag
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Book> Books { get; set; } = new List<Book>();
}