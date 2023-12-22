using System.CommandLine;
using Blogrolling.Data.RSS;
using Blogrolling.Data.RSS.Extensions;
using Blogrolling.Database;
using Blogrolling.Database.Sources;
using CodeHollow.FeedReader;
using Microsoft.EntityFrameworkCore;

var rootCommand = new RootCommand("RSS DataSource for Blogrolling.");

var addLinkArgument = new Argument<string>("link", "A RSS feed link.");
var addCommand = new Command("add", "Add a RSS feed link to database.");
addCommand.AddArgument(addLinkArgument);

var removeLinkArgument = new Argument<string>("source", "A feed link or name.");
var removeCommand = new Command("remove", "Remove a RSS feed link to database.");
removeCommand.AddArgument(removeLinkArgument);

var forceRefreshOption = new Option<bool>("--force", "Force refresh.");
var linkOption = new Option<string>("--source", "Specify a feed link or name.");
var refreshCommand = new Command("refresh", "Refresh RSS information.");
refreshCommand.AddOption(linkOption);
refreshCommand.AddOption(forceRefreshOption);

var now = DateTime.Now;
var context = Helper.GetContext();

addCommand.SetHandler(AddLink, addLinkArgument);
removeCommand.SetHandler(RemoveLink, removeLinkArgument);
refreshCommand.SetHandler(Refresh, linkOption, forceRefreshOption);

rootCommand.AddCommand(addCommand);
rootCommand.AddCommand(removeCommand);
rootCommand.AddCommand(refreshCommand);

return rootCommand.Invoke(args);

void AddLink(string link)
{
    if (!Helper.IsUrl(link))
    {
        Console.WriteLine("Link is not a valid HTTP/HTTPS url!");
        return;
    }

    var feed = RSSParser.Fetch(link);

    if (context.RSSDataSources.Any(s => s.Link == link))
    {
        Console.WriteLine("This source is already exists in database!");
        return;
    }
    
    DoRefresh(feed, link, true);
    
    Console.WriteLine("Successfully added source!");
}

void RemoveLink(string link)
{
    if (Helper.IsUrl(link))
    {
        var source = context.RSSDataSources.Include(dataSource => dataSource.Blog)
            .FirstOrDefault(d => d.Link == link);
        if (source is null)
        {
            Console.WriteLine("No such source!");
            return;
        }

        source.Blog.Source = null;
        context.RSSDataSources.Remove(source);
        
        context.SaveChanges();
        
        Console.WriteLine("Successfully removed!");
    }
    else
    {
        var blogs = context.Blogs.Where(b => b.Name.StartsWith(link));
        if (blogs.Count() > 1)
        {
            Console.WriteLine("More than 1 match: ");
            foreach (var blog in blogs)
            {
                Console.WriteLine(blog.Name);
            }
            Console.WriteLine("Please specify one of above.");
        }
        else if (!blogs.Any())
        {
            Console.WriteLine("No such blog!");
        }
        else
        {
            var blog = blogs.Include(blog => blog.Source).First();
            if (blog.Source != null)
            {
                context.DataSources.Remove(blog.Source);
                blog.Source = null;
                context.SaveChanges();
                Console.WriteLine("Successful removed source!");
            }
            else
            {
                Console.WriteLine("This blog already has no source!");
            }
        }
    }
}

void Refresh(string link = "", bool force = false)
{
    if (string.IsNullOrWhiteSpace(link))
    {
        foreach (var source in context.RSSDataSources.Where(d => d.Status == DataSourceStatus.Ok).ToList())
        {
            if (force || source.NextFetchTime < now)
            {
                var feed = RSSParser.Fetch(source.Link);   
                DoRefresh(feed, source.Link);
            }
        }
    }
    else
    {
        if (Helper.IsUrl(link))
        {
            var source = context.RSSDataSources.FirstOrDefault(d => d.Link == link);
            if (source is null)
            {
                Console.WriteLine("This source is not exists!");
                return;
            }
        }
        else
        {
            var blogs = context.Blogs.Where(b => b.Name.StartsWith(link) 
                                            && b.Source != null 
                                            && b.Source.Status == DataSourceStatus.Ok);
            foreach (var blog in blogs.Include(blog => blog.Source).ToList()) 
            {
                if (blog.Source!.Type == DataSourceType.RSS && blog.Source is RSSDataSource source)
                {
                    if (force || source.NextFetchTime < now)
                    {
                        var feed = RSSParser.Fetch(blog.Source!.Link);
                        DoRefresh(feed, source.Link);
                    }
                }
            }
        }
    }
    
    Console.WriteLine("Successfully refreshed!");
}

DataSource CreateDataSource(Feed feed, FeedAdditionalInfo info, string link)
{
    var dataSource = new RSSDataSource
    {
        Link = link, 
        Type = DataSourceType.RSS, 
        LastUpdateTime = feed.LastUpdatedDate,
        PrevFetchTime = now,
        UpdateFrequency = info.SyUpdateFrequency,
        NextFetchTime = info.NextFetchTime
    };
    context.RSSDataSources.Add(dataSource);
    
    return dataSource;
}

DataSource UpdateDataSource(Feed feed, FeedAdditionalInfo info, string link)
{
    var dataSource = context.RSSDataSources.FirstOrDefault(s => s.Link == link);

    if (dataSource is null)
    {
        throw new Exception("Source is null!");
    }
    
    if (dataSource.LastUpdateTime != feed.LastUpdatedDate)
    {
        dataSource.LastUpdateTime = feed.LastUpdatedDate;
    }

    if (dataSource.PrevFetchTime is not null && dataSource.PrevFetchTime.Value < now)
    {
        dataSource.PrevFetchTime = now;
    }

    if (info.SyUpdateFrequency is not null && dataSource.UpdateFrequency != info.SyUpdateFrequency)
    {
        dataSource.UpdateFrequency = info.SyUpdateFrequency;
    }

    if (dataSource.NextFetchTime < info.NextFetchTime)
    {
        dataSource.NextFetchTime = info.NextFetchTime;
    }
    
    return dataSource;
}

Blog UpdateOrCreateBlog(Feed feed, string link, bool createDataSource = false)
{
    var info = feed.GetAdditionalInfo();
    
    var blog = context.Blogs.Include(blog => blog.Source)
        .FirstOrDefault(b => b.Guid == feed.Link);
    if (blog is null)
    {
        blog = new Blog
        {
            Name = feed.Title,
            Description = feed.Description,
            Link = info.Link,
            Guid = info.Link,
            Source = createDataSource ? CreateDataSource(feed, info, link) : null
        };
        context.Blogs.Add(blog);
    }
    else
    {
        if (blog.Name != feed.Title)
        {
            blog.Name = feed.Title;
        }

        if (blog.Description != feed.Description)
        {
            blog.Description = feed.Description;
        }

        if (blog.Link != info.Link)
        {
            blog.Link = info.Link;
        }

        if (blog.Source is null)
        {
            if (createDataSource)
            {
                blog.Source = CreateDataSource(feed, info, link);
            }
        }
        else
        {
            blog.Source = UpdateDataSource(feed, info, link);
        }
    }

    return blog;
}

void DoRefresh(Feed feed, string link, bool createDataSource = false)
{
    var blog = UpdateOrCreateBlog(feed, link, createDataSource);

    var tagsToAdd = new List<Tag>();
    
    foreach (var item in feed.Items)
    {
        var updateTime = item.GetUpdateTime(feed.Type);
        var dirty = false;
        
        var tags = new List<Tag>();
        foreach (var category in item.GetCategories(feed.Type))
        {
            var tag = context.Tags.FirstOrDefault(t => t.Blog == blog && t.Guid == (category.Guid ?? category.Name))
                      ?? tagsToAdd.FirstOrDefault(t => t.Guid == (category.Guid ?? category.Name));

            if (tag is null)
            {
                tag = new Tag
                {
                    Name = Uri.UnescapeDataString(category.Name),
                    Guid = category.Guid ?? category.Name,
                    Link = category.Link,
                    Blog = blog
                };
                tagsToAdd.Add(tag);
                dirty = true;
            }

            tags.Add(tag);
        }

        var post = context.Posts.FirstOrDefault(p => p.Guid == item.Id);
        
        if (post is null)
        {
            post = new Post
            {
                Title = item.Title,
                Description = item.Description,
                Author = item.Author,
                Link = item.Link,
                Guid = item.Id,
                Tags = tags,
                Blog = blog,
                PublishTime = item.PublishingDate ?? now
            };
            
            context.Posts.Add(post);
        }

        if (post.Title != item.Title)
        {
            post.Title = item.Title;
            dirty = true;
        }

        if (post.Description != item.Description)
        {
            post.Description = item.Description;
            dirty = true;
        }
        
        if (post.Link != item.Link)
        {
            post.Link = item.Link;
            dirty = true;
        }

        var author = item.GetAuthor(feed.Type);
        if (post.Author != author)
        {
            post.Author = author;
            dirty = true;
        }

        if (!dirty && updateTime is not null)
        {
            post.UpdateTime = updateTime;
        }

        if (dirty)
        {
            post.UpdateTime = now;
        }
    }
    
    context.SaveChanges();

    Console.WriteLine($"Blog {blog.Name} was refreshed.");
}
