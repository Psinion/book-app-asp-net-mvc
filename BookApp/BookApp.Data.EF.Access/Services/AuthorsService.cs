using BookApp.Data.EF.Access.Contexts;
using BookApp.Domain.Entities;
using BookApp.Domain.Services;

namespace BookApp.Data.EF.Access.Services;

public class AuthorsService : IAuthorsService
{
    private readonly MainDbContext context;
    
    public AuthorsService(MainDbContext context)
    {
        this.context = context;
    }
    
    public async Task AddAuthors(List<Author> authors)
    {
        foreach (var author in authors)
        {
            author.CreatedAt = DateTime.UtcNow;
            author.CreatedBy = 1;
        }
        
        context.Authors.AddRange(authors);
        await context.SaveChangesAsync();
    }
}
