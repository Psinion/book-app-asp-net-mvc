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
    
    public static List<Review> GenerateReviews(int count)
    {
        var reviews = new Faker<Review>()
                .RuleFor(x => x.VoterName, f => f.Person.FirstName)
                .RuleFor(x => x.NumStars, f => f.Random.Int(1, 5))
                .RuleFor(x => x.Comment, f => f.Lorem.Sentence(f.Random.Int(2, 30)))
            ;
        
        return reviews.Generate(count);
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
    
    public static async Task CreateReviewsForBooks(MainDbContext context)
    {
        var reviews = GenerateReviews(1000);
        var reviewsCount = reviews.Count;
        var bookService = new BooksService(context);
        
        var random = new Random();
        
        var books = await bookService.GetAllBooksTracked();
        foreach (var book in books)
        {
            var reviewsCountForBook = random.Next(0, 10);
            var reviewIndex = 0;
            
            while (reviewIndex < reviewsCountForBook)
            {
                var randomReview = reviews[random.Next(reviewsCount)];
                book.Reviews.Add(randomReview);
                reviewIndex++;
            }
        }

        await context.SaveChangesAsync();
    }
    
    public static async Task CreatePriceOfferForBooks(MainDbContext context)
    {
        var bookService = new BooksService(context);
        
        var random = new Random();
        
        var books = await bookService.GetAllBooksTracked();
        foreach (var book in books)
        {
            var chance = random.Next(0, 10);
            if (chance < 0.3f)
            {
                var priceOffer = new Faker<PriceOffer>()
                        .RuleFor(x => x.NewPrice, f => f.Random.Decimal(50, book.Price / 2))
                        .RuleFor(x => x.PromotionalText, f => f.Lorem.Sentence(f.Random.Int(1, 4)))
                    ;
                book.Promotion = priceOffer;
            }
        }

        await context.SaveChangesAsync();
    }
    
    public static async Task CreateTagsForBooks(MainDbContext context)
    {
        var tagsService = new TagsService(context);
        var bookService = new BooksService(context);
        
        var random = new Random();

        var tagsDictionary = new Dictionary<string, Tag>();
        
        var tags = new Faker<Tag>()
                .RuleFor(x => x.Name, f => f.Lorem.Sentence(f.Random.Int(1, 2)))
                .Generate(30)
            ;
        foreach (var tag in tags)
        {
            tagsDictionary.TryAdd(tag.Name, tag);
        }

        await tagsService.AddTags(tagsDictionary.Values.ToList());
        
        var savedTags = context.Tags.ToList();
        
        var books = await bookService.GetAllBooksTracked();
        foreach (var book in books)
        {
            var tagsIds = new List<long>(3);
            var tagsCount = random.Next(1, 3);
            var tagIndex = 0;
            
            while (tagIndex < tagsCount)
            {
                var tagToAdd = savedTags[random.Next(0, savedTags.Count)];
                if (!tagsIds.Contains(tagToAdd.Id))
                {
                    tagsIds.Add(tagToAdd.Id);
                    book.Tags.Add(tagToAdd);
                    tagIndex++;
                }
            }
        }

        await context.SaveChangesAsync();
    }
}