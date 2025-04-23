using System.ComponentModel.DataAnnotations;
using BookApp.Domain.Entities.Base.Interfaces;

namespace BookApp.Domain.Entities;

public class Author : IAuditable, ISoftDeletable
{
    public long Id { get; private set; }
    
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [MaxLength(100)]
    public string? Patronymic { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }

    public ICollection<BookAuthor> BooksLink { get; set; } = new List<BookAuthor>();
}