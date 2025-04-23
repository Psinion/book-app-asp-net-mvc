using BookApp.Domain.Entities;

namespace BookApp.Domain.Services;

public interface IAuthorsService
{
    public Task AddAuthors(List<Author> authors);
}