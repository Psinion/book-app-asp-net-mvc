using BookApp.Domain.Entities;

namespace BookApp.Domain.Services;

public interface ITagsService
{
    Task AddTags(List<Tag> tags);
}