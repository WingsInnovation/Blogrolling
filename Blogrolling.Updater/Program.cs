using System.CommandLine;
using Blogrolling.Database.Enums;
using Blogrolling.Updater;
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

addCommand.SetHandler(l => Task.FromResult(AddLink(l)), addLinkArgument);
removeCommand.SetHandler(l => Task.FromResult(RemoveLink(l)), removeLinkArgument);
refreshCommand.SetHandler((l, f) => Task.FromResult(Refresh(l, f)), linkOption, forceRefreshOption);

rootCommand.AddCommand(addCommand);
rootCommand.AddCommand(removeCommand);
rootCommand.AddCommand(refreshCommand);

try
{
    return rootCommand.Invoke(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    return (int)EnumExitCode.Error;
}

int AddLink(string link)
{
    if (!Helper.IsUrl(link))
    {
        Console.WriteLine("Link is not a valid HTTP/HTTPS url!");
        return (int)EnumExitCode.InvalidUrl;
    }

    var feed = RSSParser.Fetch(link);

    if (feed is null)
    {
        Console.WriteLine("This link can not be reached!");
        return (int)EnumExitCode.CantReach;
    }

    var source = context.RSSDataSources.FirstOrDefault(s => s.Link == link);
    if (source != null)
    {
        if (source.Status == DataSourceStatus.Ok)
        {
            Console.WriteLine("This source is already exists in database!");
            return (int)EnumExitCode.AlreadyExists;
        }

        source.Status = DataSourceStatus.Ok;
        context.SaveChanges();
    }
    
    DoRefresh(feed, link, true);
    
    Console.WriteLine("Successfully added source!");
    return (int)EnumExitCode.Success;
}

int RemoveLink(string link)
{
    if (Helper.IsUrl(link))
    {
        var source = context.RSSDataSources.Include(dataSource => dataSource.Blog)
            .FirstOrDefault(d => d.Link == link);
        if (source is null)
        {
            Console.WriteLine("No such source!");
            return (int)EnumExitCode.SourceNotFound;
        }

        source.Blog.Source = null;
        context.RSSDataSources.Remove(source);
        
        context.SaveChanges();
        
        Console.WriteLine("Successfully removed!");
        return (int)EnumExitCode.Success;
    }

    var blogs = context.Blogs.Where(b => b.Name.StartsWith(link));
    if (blogs.Count() > 1)
    {
        Console.WriteLine("More than 1 match: ");
        foreach (var b in blogs)
        {
            Console.WriteLine(b.Name);
        }
        Console.WriteLine("Please specify one of above.");
        return (int)EnumExitCode.Success;
    }

    if (!blogs.Any())
    {
        Console.WriteLine("No such blog!");
        return (int)EnumExitCode.SourceNotFound;
    }

    var blog = blogs.Include(blog => blog.Source).First();
    if (blog.Source != null)
    {
        context.DataSources.Remove(blog.Source);
        blog.Source = null;
        context.SaveChanges();
        Console.WriteLine("Successful removed source!");
        return (int)EnumExitCode.Success;
    }

    Console.WriteLine("This blog already has no source!");
    return (int)EnumExitCode.SourceNotFound;
}

int Refresh(string link = "", bool force = false)
{
    if (string.IsNullOrWhiteSpace(link))
    {
        foreach (var source in context.RSSDataSources.Where(d => d.Status == DataSourceStatus.Ok).ToList())
        {
            if (force || source.NextFetchTime < now)
            {
                var feed = RSSParser.Fetch(source.Link);   
                if (feed is null)
                {
                    source.Status = DataSourceStatus.Invalid;
                    context.SaveChanges();
                    Console.WriteLine($"Source {source.Link} can't be reached!");
                    continue;
                }
                
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
                return (int)EnumExitCode.SourceNotFound;
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
                        if (feed is null)
                        {
                            source.Status = DataSourceStatus.Invalid;
                            context.SaveChanges();
                            Console.WriteLine($"Source {source.Link} can't be reached!");
                            continue;
                        }
                        
                        DoRefresh(feed, source.Link);
                    }
                }
            }
        }
    }
    
    Console.WriteLine("Successfully refreshed!");
    return (int)EnumExitCode.Success;
}
