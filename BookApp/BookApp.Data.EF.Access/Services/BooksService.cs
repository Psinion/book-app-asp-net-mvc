using BookApp.Data.EF.Access.Contexts;
using BookApp.Domain.Entities;
using BookApp.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Data.EF.Access.Services;

public class BooksService : IBooksService
{
    private readonly MainDbContext context;
    
    public BooksService(MainDbContext context)
    {
        this.context = context;
    }
    
    public async Task<List<Book>> GetAllBooks()
    {
        return await context.Books.AsNoTracking().ToListAsync();
    }
    
    public async Task<List<Book>> GetAllBooksTracked()
    {
        return await context.Books.ToListAsync();
    }
    
    public async Task AddBooks(List<Book> books)
    {
        foreach (var book in books)
        {
            book.CreatedAt = DateTime.UtcNow;
            book.CreatedBy = 1;
        }
        
        context.Books.AddRange(books);
        await context.SaveChangesAsync();
    }
}