using Bogus;
using BookApp.Data.EF.Access.Contexts;
using BookApp.Data.EF.Access.Services;
using BookApp.Domain.Entities;

namespace BookApp.DataGenerators;

public static class DataGenerator
{
    public static List<Book> GenerateBooks(int count)
    {
        var books = new Faker<Book>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence(f.Random.Int(10, 20)))
            .RuleFor(x => x.PublishedOn, f => f.Date.Recent().ToUniversalTime())
            .RuleFor(x => x.Publisher, f => f.Name.FullName().OrNull(f, 0.2f))
            .RuleFor(x => x.Price, f => f.Random.Decimal(100, 3000))
            .RuleFor(x => x.ImageUrl, f => f.Image.PicsumUrl().OrNull(f, 0.2f))
            ;
        
        return books.Generate(count);
    }
    
    public static List<Author> GenerateAuthors(int count)
    {
        var authors = new Faker<Author>()
            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.Patronymic, f => f.Person.FullName)
            ;
        
        return authors.Generate(count);
    }

    public static async Task CreateBooks(MainDbContext context)
    {
        var bookService = new BooksService(context);
        var books = DataGenerator.GenerateBooks(1000);
        await bookService.AddBooks(books);
    }    
    
    public static async Task CreateAuthorsForBooks(MainDbContext context)
    {
        var authors = GenerateAuthors(1000);
        var authorsCount = authors.Count;
        var bookService = new BooksService(context);
        
        var random = new Random();
        
        var books = await bookService.GetAllBooksTracked();
        foreach (var book in books)
        {
            var existingAuthorIds = new HashSet<long>(
                book.AuthorsLink.Select(ba => ba.AuthorId)
            );
            
            var authorsCountForBook = random.Next(1, 5);
            var attempts = 0;
            
            while (existingAuthorIds.Count < authorsCountForBook && attempts < 10)
            {
                var randomAuthor = authors[random.Next(authorsCount)];
                if (existingAuthorIds.Add(randomAuthor.Id))
                {
                    book.AuthorsLink.Add(new BookAuthor
                    {
                        Author = randomAuthor,
                        BookId = book.Id
                    });
                }
                attempts++;
            }
            
            
        }

        await context.SaveChangesAsync();
    }
}