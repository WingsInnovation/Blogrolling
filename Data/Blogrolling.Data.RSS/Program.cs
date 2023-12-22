using System.CommandLine;
using Blogrolling.Data.RSS;
using Blogrolling.Database;
using Blogrolling.Database.Sources;
using Blogrolling.Utilities;
using CodeHollow.FeedReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
var context = GetContext();

addCommand.SetHandler(AddLink, addLinkArgument);
removeCommand.SetHandler(RemoveLink, removeLinkArgument);
refreshCommand.SetHandler(Refresh, linkOption, forceRefreshOption);

rootCommand.AddCommand(addCommand);
rootCommand.AddCommand(removeCommand);
rootCommand.AddCommand(refreshCommand);

return rootCommand.Invoke(args);

void AddLink(string link)
{
    // var context = GetContext();
    
    if (!IsUrl(link))
    {
        Console.WriteLine("Link is not a valid HTTP/HTTPS url!");
        return;
    }

    var (feed, info) = RSSParser.Fetch(link);

    if (context.RSSDataSources.Any(s => s.Link == link))
    {
        Console.WriteLine("This source is already exists in database!");
        return;
    }
    
    DoRefresh(feed, info, link, true);
    
    Console.WriteLine("Successfully added source!");
}

void RemoveLink(string link)
{
    // using var context = GetContext();
    
    if (IsUrl(link))
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
    // using var context = GetContext();
    
    if (string.IsNullOrWhiteSpace(link))
    {
        foreach (var source in context.RSSDataSources.Where(d => d.Status == DataSourceStatus.Ok))
        {
            if (force || source.NextFetchTime < now)
            {
                var (feed, info) = RSSParser.Fetch(source.Link);   
                DoRefresh(feed, info, source.Link);
            }
        }
    }
    else
    {
        if (IsUrl(link))
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
            foreach (var blog in blogs.Include(blog => blog.Source)) 
            {
                if (blog.Source!.Type == DataSourceType.RSS && blog.Source is RSSDataSource source)
                {
                    if (force || source.NextFetchTime < now)
                    {
                        var (feed, info) = RSSParser.Fetch(blog.Source!.Link);
                        DoRefresh(feed, info, source.Link);
                    }
                }
            }
        }
    }
    
    Console.WriteLine("Successfully refreshed!");
}

DataSource CreateDataSource(Feed feed, FeedAdditionalInfo info, string link)
{
    // using var context = GetContext();
    
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
    // using var context = GetContext();
    
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
    
    // context.SaveChanges();
    
    return dataSource;
}

Blog UpdateOrCreateBlog(Feed feed, FeedAdditionalInfo info, string link, bool createDataSource = false)
{
    // using var context = GetContext();
    
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

    // context.SaveChanges();
    
    return blog;
}

void DoRefresh(Feed feed, FeedAdditionalInfo info, string link, bool createDataSource = false)
{
    // using var context = GetContext();
    
    var blog = UpdateOrCreateBlog(feed, info, link, createDataSource);
    
    foreach (var item in feed.Items)
    {
        if (context.Posts.Any(p => p.Guid == item.Id))
        {
            continue;
        }

        var tags = new List<Tag>();
        foreach (var category in item.Categories)
        {
            var tag = context.Tags.FirstOrDefault(t => t.Name == category);
            if (tag is null)
            {
                tag = new Tag
                {
                    Name = category
                };
                context.Tags.Add(tag);
                context.SaveChanges();
            }

            tags.Add(tag);
        }

        var post = new Post
        {
            Title = item.Title,
            Description = item.Description,
            Author = item.Author,
            Link = item.Link,
            Guid = item.Id,
            Tags = tags,
            Blog = blog
        };
        context.Posts.Add(post);
    }
    
    context.SaveChanges();

    Console.WriteLine($"Blog {blog.Name} was refreshed.");
}

bool IsUrl(string str)
{
    return Uri.TryCreate(str, UriKind.Absolute, out var result)
           && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
}

BlogrollingContext GetContext()
{
    var connectionString = ConfigHelper.GetConnectionString();
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception("Please place the ConnectionString into '%USER_PROFILE%/.config/blogrolling/connectionString'.");
    }
    
    var optionsBuilder = new DbContextOptionsBuilder<BlogrollingContext>();
    optionsBuilder.UseLazyLoadingProxies()
        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    return new BlogrollingContext(optionsBuilder.Options);
}
