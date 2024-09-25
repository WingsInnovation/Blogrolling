using Blogrolling.Database;
using Blogrolling.Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Service.Impl;

public class PollingService(BlogrollingContext db) : IPollingService
{
    public async Task<Blog> PollBlog(string name, string link, string? owner = null)
    {
        var blog = await db.Blogs.FirstOrDefaultAsync(n => n.Name == name
                                                           && n.Link == link);
        if (blog is null)
        {
            blog = new Blog
            {
                Name = name,
                Link = link,
                Owner = owner
            };
            await db.Blogs.AddAsync(blog);
        }
        else
        {
            if (owner is not null && blog.Owner != owner)
            {
                blog.Owner = owner;
            }
        }

        await db.SaveChangesAsync();
        return blog;
    }

    public async Task<DataSource> PollDataSource(Blog blog, string link,
        DataSourceType type, DataSourceStatus status = DataSourceStatus.Ok)
    {
        var data = await db.DataSources.FirstOrDefaultAsync(n => n.Blog.Id == blog.Id
                                                           && n.Link == link
                                                           && n.Type == type);
        if (data is null)
        {
            data = new DataSource
            {
                Blog = blog,
                Link = link,
                Type = type,
                Status = status
            };
            await db.DataSources.AddAsync(data);
        }
        else
        {
            if (data.Type != type)
            {
                data.Type = type;
            }

            if (data.Status != status)
            {
                data.Status = status;
            }
        }

        await db.SaveChangesAsync();
        return data;
    }

    public async Task<Post> PollPost(Blog blog, string link, string title, List<Tag> tags,
        DateTime? publish, string? description = null,
        string? author = null, DateTime? update = null)
    {
        var post = await db.Posts.Include(post => post.Tags)
            .FirstOrDefaultAsync(n => n.Blog.Id == blog.Id 
                                      && n.Link == link);
        if (post is null)
        {
            post = new Post
            {
                Blog = blog,
                Link = link,
                Title = title,
                PublishTime = publish ?? DateTime.Now,
                Description = description,
                Author = author,
                UpdateTime = update,
                Tags = tags
            };
            await db.Posts.AddAsync(post);
        }
        else
        {
            if (post.Title != title)
            {
                post.Title = title;
            }

            if (publish is not null && post.PublishTime != publish)
            {
                post.PublishTime = publish.Value;
            }

            if (description is not null && post.Description != description)
            {
               post.Description = description; 
            }

            if (author is not null && post.Author != author)
            {
                post.Author = author;
            }

            if (update is not null && post.UpdateTime != update)
            {
                post.UpdateTime = update.Value;
            }

            if (!post.Tags.SequenceEqual(tags))
            {
                post.Tags = tags;
            }
        }

        await db.SaveChangesAsync();
        return post;
    }

    public async Task<Tag> PollTag(Blog blog, string name, string? link = null)
    {
        var tag = await db.Tags.FirstOrDefaultAsync(n => n.Blog.Id == blog.Id 
                                                         && n.Name == name);
        if (tag is null)
        {
            tag = new Tag
            {
                Blog = blog,
                Name = name,
                Link = link
            };
            await db.Tags.AddAsync(tag);
        }
        else
        {
            if (tag.Link != link)
            {
                tag.Link = link;
            }
        }

        await db.SaveChangesAsync();
        return tag;
    }
}