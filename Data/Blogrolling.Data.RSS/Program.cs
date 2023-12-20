using System.CommandLine;
using System.Text;
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

addCommand.SetHandler(AddLink, linkArgument);
removeCommand.SetHandler(RemoveLink, linkArgument);
refreshCommand.SetHandler(Refresh, linkArgument, forceRefreshOption);

rootCommand.AddCommand(addCommand);
rootCommand.AddCommand(removeCommand);
rootCommand.AddCommand(refreshCommand);

return await rootCommand.InvokeAsync(args);

async void AddLink(string link)
{
    var db = GetContext();

    if (!IsUrl(link))
    {
        Console.WriteLine("Link is not a valid HTTP/HTTPS url!");
        return;
    }

    var (feed, info) = await RSSParser.DoFetch(link);
    // Todo
    
    var dataSource = new RSSDataSource
    {
        Link = link, 
        Type = DataSourceType.RSS, 
        LastUpdateTime = feed.LastUpdatedDate,
        PrevFetchTime = DateTime.Now,
        UpdateFrequency = info.SyUpdateFrequency,
        NextFetchTime = info.NextFetchTime
    };
    db.RSSDataSources.Add(dataSource);
    await db.SaveChangesAsync();
}

async void RemoveLink(string link)
{
    
}

async void Refresh(string link, bool force)
{
    
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
    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    return new BlogrollingContext(optionsBuilder.Options);
}
