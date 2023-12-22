using CodeHollow.FeedReader;

namespace Blogrolling.Data.RSS;

public class RSSParser
{
    public static Feed Fetch(string link)
    {
        return FeedReader.ReadAsync(link, autoRedirect: true).Result;
    }
}