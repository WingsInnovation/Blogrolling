using System.Xml.Linq;
using CodeHollow.FeedReader;

namespace Blogrolling.Data.RSS;

public class RSSParser
{
    public static async Task<(Feed Feed, FeedAdditionalInfo AdditionalInfo)> DoFetch(string link)
    {
        var feed = await FeedReader.ReadAsync(link, autoRedirect: true);
        var rootNode = feed.SpecificFeed.Element.Document!.Root;
        throw new NotImplementedException();
        // rootNode.Element(new XName())
    }
}