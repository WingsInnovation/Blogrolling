using Blogrolling.Database;
using Blogrolling.Database.Enums;

namespace Blogrolling.Service;

public interface IPollingService
{
    public Task<Blog> PollBlog(string name, string link, string? owner = null);
    public Task<DataSource> PollDataSource(Blog blog, string link, DataSourceType type, 
        DataSourceStatus status = DataSourceStatus.Ok);
    public Task<Post> PollPost(Blog blog, string link, string title, List<Tag> tags,
        DateTime? publish, string? description = null, 
        string? author = null, DateTime? update = null);
    public Task<Tag> PollTag(Blog blog, string name, string? link = null);
}