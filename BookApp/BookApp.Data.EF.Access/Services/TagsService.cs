using BookApp.Data.EF.Access.Contexts;
using BookApp.Domain.Entities;
using BookApp.Domain.Services;

namespace BookApp.Data.EF.Access.Services;

public class TagsService : ITagsService
{
    private readonly MainDbContext context;
    
    public TagsService(MainDbContext context)
    {
        this.context = context;
    }
    
    public async Task AddTags(List<Tag> tags)
    {
        context.Tags.AddRange(tags);
        await context.SaveChangesAsync();
    }
}