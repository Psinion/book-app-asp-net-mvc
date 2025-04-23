using BookApp.Domain.Entities.Base.Interfaces;

namespace BookApp.Domain.Entities;

public class Review : IAuditable, ISoftDeletable
{
    public long Id { get; private set; }
    
    public long BookId { get; set; }
    
    public string VoterName { get; set; } = string.Empty;
    public int NumStars { get; set; }
    public string? Comment { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }
}