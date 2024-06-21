using Blogrolling.Database;
using Blogrolling.Database.Enums;

namespace Blogrolling;

public interface IRollingService
{
    public Blog AddBlog(string name, string url, string? owner = null);
    public DataSource AddDataSource(Blog blog, string link, DataSourceType type, 
        DataSourceStatus status = DataSourceStatus.Ok,
        DateTime? lastUpdateTime = null, DateTime? prevFetchTime = null, DateTime? nextFetchTime = null);
    public Post AddPost(Blog blog, string title, string link, DateTime publish, 
        string? description = null, string? author = null, DateTime? update = null);
    public Tag AddTag(Blog blog, string name, string? link = null);

    public Blog? GetBlog(int id);
    public Blog? GetBlog(string link);
    public IEnumerable<Blog> GetBlogs(string? name, string? link, string? owner);

    public IEnumerable<Post> GetPosts(string? title, string? desc, string? link, string? author);

    public IEnumerable<Tag> GetTags(string? name);
}