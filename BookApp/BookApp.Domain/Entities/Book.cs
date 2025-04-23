using System.ComponentModel.DataAnnotations;
using BookApp.Domain.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Domain.Entities;

public class Book : IAuditable, ISoftDeletable
{
    public long Id { get; private set; }
    public required string Title { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    public DateTime PublishedOn { get; set; }
    
    [MaxLength(100)]
    public string? Publisher { get; set; }
    
    [Precision(10, 2)]
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }
    
    public PriceOffer? Promotion { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<BookAuthor> AuthorsLink { get; set; } = new List<BookAuthor>();
}