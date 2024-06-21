using Blogrolling.Database;
using Blogrolling.Database.Enums;

namespace Blogrolling;

public class Rolling(BlogrollingContext db) : IRollingService
{
    public Blog AddBlog(string name, string url, string? owner = null)
    {
        var blog = new Blog
        {
            Name = name,
            Link = url,
            Owner = owner
        };
        db.Blogs.Add(blog);
        return blog;
    }

    public DataSource AddDataSource(Blog blog, string link, DataSourceType type, 
        DataSourceStatus status = DataSourceStatus.Ok,
        DateTime? lastUpdateTime = null, DateTime? prevFetchTime = null, DateTime? nextFetchTime = null)
    {
        var dataSource = new DataSource
        {
            Status = status,
            Type = type,
            Link = link,
            LastUpdateTime = lastUpdateTime,
            PrevFetchTime = prevFetchTime,
            NextFetchTime = nextFetchTime,
            Blog = blog
        };
        db.DataSources.Add(dataSource);
        return dataSource;
    }

    public Post AddPost(Blog blog, string title, string link, DateTime publish, 
        string? description = null, string? author = null,
        DateTime? update = null)
    {
        var post = new Post
        {
            Title = title,
            Link = link,
            Description = description,
            Author = author,
            PublishTime = publish,
            UpdateTime = update,
            Blog = blog
        };
        db.Posts.Add(post);
        return post;
    }

    public Tag AddTag(Blog blog, string name, string? link = null)
    {
        var tag = new Tag
        {
            Name = name,
            Link = link,
            Blog = blog
        };
        db.Tags.Add(tag);
        return tag;
    }

    public Blog? GetBlog(int id)
    {
        return db.Blogs.FirstOrDefault(b => b.Id == id);
    }

    public Blog? GetBlog(string link)
    {
        return db.Blogs.FirstOrDefault(b => b.Link == link);
    }

    public IEnumerable<Blog> GetBlogs(string? name, string? link, string? owner)
    {
        return db.Blogs
            .Where(b => string.IsNullOrWhiteSpace(name) || b.Name.Contains(name))
            .Where(b => string.IsNullOrWhiteSpace(link) || b.Link == link)
            .Where(b => string.IsNullOrWhiteSpace(owner) || b.Owner == owner)
            .ToList();
    }

    public IEnumerable<Post> GetPosts(string? title, string? desc, string? link, string? author)
    {
        return db.Posts
            .Where(p => string.IsNullOrWhiteSpace(title) || p.Title.Contains(title))
            .Where(p => string.IsNullOrWhiteSpace(desc) || string.IsNullOrWhiteSpace(p.Description) || p.Description.Contains(desc))
            .Where(p => string.IsNullOrWhiteSpace(link) || p.Link == link)
            .Where(p => string.IsNullOrWhiteSpace(author) || p.Author == author)
            .ToList();
    }

    public IEnumerable<Tag> GetTags(string? name)
    {
        return db.Tags
            .Where(b => string.IsNullOrWhiteSpace(name) || b.Name.Contains(name))
            .ToList();
    }
}