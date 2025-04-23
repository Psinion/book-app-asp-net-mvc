using BookApp.Domain.Entities;

namespace BookApp.Domain.Services;

public interface IBooksService
{
    Task<List<Book>> GetAllBooks();
    Task<List<Book>> GetAllBooksTracked();
    Task AddBooks(List<Book> books);
}