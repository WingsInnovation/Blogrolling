using System.CommandLine;
using Blogrolling.Data.RSS;
using Blogrolling.Database;
using Blogrolling.Database.Sources;
using Blogrolling.Utilities;
using CodeHollow.FeedReader;
using Microsoft.EntityFrameworkCore;

var rootCommand = new RootCommand("RSS DataSource for Blogrolling.");

var linkArgument = new Argument<string>("link", "A RSS feed link.");

var addCommand = new Command("add", "Add a RSS feed link to database.");
addCommand.AddArgument(linkArgument);

var removeCommand = new Command("remove", "Remove a RSS feed link to database.");
removeCommand.AddArgument(linkArgument);

var forceRefreshOption = new Option<bool>("--force", "Force refresh.");
var refreshCommand = new Command("refresh", "Refresh RSS information.");
refreshCommand.AddArgument(linkArgument);
refreshCommand.AddOption(forceRefreshOption);

var now = DateTime.Now;
var db = GetContext();

addCommand.SetHandler(AddLink, linkArgument);
removeCommand.SetHandler(RemoveLink, linkArgument);
refreshCommand.SetHandler(Refresh, linkArgument, forceRefreshOption);

rootCommand.AddCommand(addCommand);
rootCommand.AddCommand(removeCommand);
rootCommand.AddCommand(refreshCommand);

return await rootCommand.InvokeAsync(args);

async void AddLink(string link)
{
    if (!IsUrl(link))
    {
        Console.WriteLine("Link is not a valid HTTP/HTTPS url!");
        return;
    }

    var (feed, info) = await RSSParser.Fetch(link);

    if (await db.Blogs.AnyAsync(b => b.Guid == info.Link && b.Source != null))
    {
        Console.WriteLine("This source is already exists in database!");
        return;
    }

    await UpdateOrCreateBlog(feed, info, true);
    
    DoRefresh(feed, info);
    
    Console.WriteLine("Successfully added source!");
}

async void RemoveLink(string link)
{
    if (IsUrl(link))
    {
        var source = await db.RSSDataSources.Include(dataSource => dataSource.Blog)
            .FirstOrDefaultAsync(d => d.Link == link);
        if (source is null)
        {
            Console.WriteLine("No such source!");
            return;
        }

        source.Blog.Source = null;
        db.RSSDataSources.Remove(source);
        
        
        await db.SaveChangesAsync();
        
        Console.WriteLine("Successfully removed!");
    }
    else
    {
        var blogs = db.Blogs.Where(b => b.Name.StartsWith(link));
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
                db.DataSources.Remove(blog.Source);
                blog.Source = null;
                await db.SaveChangesAsync();
                Console.WriteLine("Successful removed source!");
            }
            else
            {
                Console.WriteLine("This blog already has no source!");
            }
        }
    }
}

async void Refresh(string link = "", bool force = false)
{
    if (string.IsNullOrWhiteSpace(link))
    {
        foreach (var source in db.RSSDataSources.Where(d => d.Status == DataSourceStatus.Ok))
        {
            if (force || source.NextFetchTime < now)
            {
                var (feed, info) = await RSSParser.Fetch(link);   
                DoRefresh(feed, info);
            }
        }
    }
    else
    {
        if (IsUrl(link))
        {
            var source = await db.RSSDataSources.Where(d => d.Link == link).FirstOrDefaultAsync();
            if (source is null)
            {
                Console.WriteLine("This source is not exists!");
                return;
            }
        }
        else
        {
            var blogs = db.Blogs.Where(b => b.Name.StartsWith(link) 
                                            && b.Source != null 
                                            && b.Source.Status == DataSourceStatus.Ok);
            foreach (var blog in blogs.Include(blog => blog.Source)) 
            {
                if (blog.Source!.Type == DataSourceType.RSS && blog.Source is RSSDataSource source)
                {
                    if (force || source.NextFetchTime < now)
                    {
                        var (feed, info) = await RSSParser.Fetch(blog.Source!.Link);
                        DoRefresh(feed, info);
                    }
                }
            }
        }
    }
    
    Console.WriteLine("Successfully refreshed!");
}

async Task<DataSource> UpdateOrCreateDataSource(Feed feed, FeedAdditionalInfo info)
{
    var dataSource = await db.RSSDataSources.FirstOrDefaultAsync(s => s.Link == feed.Link);

    if (dataSource is null)
    {
        dataSource = new RSSDataSource
        {
            Link = feed.Link, 
            Type = DataSourceType.RSS, 
            LastUpdateTime = feed.LastUpdatedDate,
            PrevFetchTime = now,
            UpdateFrequency = info.SyUpdateFrequency,
            NextFetchTime = info.NextFetchTime
        };
        await db.RSSDataSources.AddAsync(dataSource);
    }
    else
    {
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
    }
    await db.SaveChangesAsync();
    
    return dataSource;
}

async Task<Blog> UpdateOrCreateBlog(Feed feed, FeedAdditionalInfo info, bool createDataSource = false)
{
    var blog = await db.Blogs.Include(blog => blog.Source)
        .FirstOrDefaultAsync(b => b.Guid == feed.Link);
    if (blog is null)
    {
        blog = new Blog
        {
            Name = feed.Title,
            Description = feed.Description,
            Link = info.Link,
            Guid = info.Link,
            Source = createDataSource ? await UpdateOrCreateDataSource(feed, info) : null
        };
        await db.Blogs.AddAsync(blog);
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

        if (blog.Source is null && createDataSource)
        {
            blog.Source = await UpdateOrCreateDataSource(feed, info);
        }
    }

    await db.SaveChangesAsync();
    
    return blog;
}

async void DoRefresh(Feed feed, FeedAdditionalInfo info)
{
    var blog = await UpdateOrCreateBlog(feed, info);
    
    foreach (var item in feed.Items)
    {
        if (await db.Posts.AnyAsync(p => p.Guid == item.Id))
        {
            continue;
        }

        var tags = new List<Tag>();
        foreach (var category in item.Categories)
        {
            var tag = await db.Tags.FirstOrDefaultAsync(t => t.Name == category);
            if (tag is null)
            {
                tag = new Tag
                {
                    Name = category
                };
                await db.Tags.AddAsync(tag);
                await db.SaveChangesAsync();
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
        await db.Posts.AddAsync(post);
        await db.SaveChangesAsync();
    }

    Console.WriteLine($"Blog {blog} was refreshed.");
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
        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    return new BlogrollingContext(optionsBuilder.Options);
}
