using System.Globalization;
using System.Xml.Linq;
using CodeHollow.FeedReader;

namespace Blogrolling.Data.RSS;

public class RSSParser
{
    public static async Task<(Feed Feed, FeedAdditionalInfo AdditionalInfo)> Fetch(string link)
    {
        var feed = await FeedReader.ReadAsync(link, autoRedirect: true);
        var document = feed.SpecificFeed.Element.Document!;

        var info = new FeedAdditionalInfo();

        if (feed.Type == FeedType.Rss)
        {
            var blogLink = (await FindElements(document, "rss", "link")).FirstOrDefault();
            if (blogLink is not null)
            {
                info.Link = blogLink.Value;
            }
        }
        else if (feed.Type == FeedType.Atom)
        {
            var blogLinks = await FindElements(document, "rss", "link");
            foreach (var blogLink in blogLinks.Where(blogLink => blogLink.Attribute("rel") == null))
            {
                info.Link = blogLink.Attribute("href")!.Value;
                break;
            }
        }
        
        var updatePeriod = (await FindElements(document, "rss", "channel", "sy:updatePeriod")).FirstOrDefault();
        if (updatePeriod is not null)
        {
            var updateFrequency = (await FindElements(document, "rss", "channel", "sy:updateFrequency")).FirstOrDefault();
            if (updateFrequency is not null)
            {
                if (int.TryParse(updateFrequency.Value, out var value))
                {
                    info.SyUpdateFrequency = ToUpdateDateTime(updatePeriod.Value, value);

                    if (info.SyUpdateFrequency is not null)
                    {
                        var updateBase = (await FindElements(document, "rss", "channel", "sy:updateBase")).FirstOrDefault();
                        if (updateBase is not null)
                        {
                            if (DateTime.TryParse(updateBase.Value, CultureInfo.InvariantCulture, out var baseDateTime))
                            {
                                var next = ToNextUpdateTime(info.SyUpdateFrequency.Value, baseDateTime);
                                if (next is not null)
                                {
                                    info.NextFetchTime = next.Value;
                                }
                            }
                        }
                    }
                }
            }
        }

        return (feed, info);
    }

    private static async Task<List<XElement>> FindElements(XContainer container, params string[] path)
    {
        var list = new List<XElement>();

        if (path.Length >= 1)
        {
            foreach (var element in container.Elements(path[0]))
            {
                list.AddRange(await FindElements(element, path[1..]));
            }
        }
        else
        {
            if (container is XElement e)
            {
                list.Add(e);
            }
        }
        
        return list;
    }

    private static DateTime? ToUpdateDateTime(string period, int frequency)
    {
        return period switch
        {
            "hourly" => new DateTime(0, 0, 0, frequency, 0, 0),
            "daily" => new DateTime(0, 0, frequency, 0, 0, 0),
            "weekly" => new DateTime(0, 0, frequency * 7, 0, 0, 0),
            "monthly" => new DateTime(0, frequency, 0, 0, 0, 0),
            "yearly" => new DateTime(frequency, 0, 0, 0, 0, 0),
            _ => null   // Period out of range, but a recover-able problem.
        };
    }
    
    private static DateTime? ToNextUpdateTime(DateTime frequency, DateTime updateBase)
    {
        var now = DateTime.Now;
        
        while (updateBase < now)
        {
            updateBase = updateBase.AddTicks(frequency.Ticks);
        }

        return updateBase;
    }
}