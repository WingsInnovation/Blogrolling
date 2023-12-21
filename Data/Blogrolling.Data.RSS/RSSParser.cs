using System.Globalization;
using System.Xml.Linq;
using CodeHollow.FeedReader;

namespace Blogrolling.Data.RSS;

public class RSSParser
{
    public static (Feed Feed, FeedAdditionalInfo AdditionalInfo) Fetch(string link)
    {
        var feed = FeedReader.ReadAsync(link, autoRedirect: true).Result;
        var document = feed.SpecificFeed.Element.Document!;

        var info = new FeedAdditionalInfo();

        if (feed.Type is FeedType.Rss or FeedType.Rss_2_0 or FeedType.Rss_1_0 or FeedType.Rss_0_92 or FeedType.Rss_0_91)
        {
            var blogLink = FindElements(document, "rss", "channel", "link").FirstOrDefault();
            if (blogLink is not null)
            {
                info.Link = blogLink.Value;
            }
        }
        else if (feed.Type == FeedType.Atom)
        {
            var blogLinks = FindElements(document, XName.Get("feed", "http://www.w3.org/2005/Atom"), XName.Get("link", "http://www.w3.org/2005/Atom"));
            foreach (var blogLink in blogLinks.Where(blogLink => blogLink.Attribute("rel") == null ||
                                                                 (blogLink.Attribute("rel") != null && blogLink.Attribute("rel")!.Value == "alternate")))
            {
                info.Link = blogLink.Attribute("href")!.Value;
                break;
            }
        }
        
        var updatePeriod = FindElements(document, "rss", "channel", XName.Get("updatePeriod", "http://purl.org/rss/1.0/modules/syndication/")).FirstOrDefault();
        if (updatePeriod is not null)
        {
            var updateFrequency = FindElements(document, "rss", "channel", XName.Get("updateFrequency", "http://purl.org/rss/1.0/modules/syndication/")).FirstOrDefault();
            if (updateFrequency is not null)
            {
                if (int.TryParse(updateFrequency.Value, out var value))
                {
                    info.SyUpdateFrequency = ToUpdateDateTime(updatePeriod.Value, value);

                    if (info.SyUpdateFrequency is not null)
                    {
                        var updateBase = FindElements(document, "rss", "channel", XName.Get("updateBase", "http://purl.org/rss/1.0/modules/syndication/")).FirstOrDefault();
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

    private static List<XElement> FindElements(XContainer container, params XName[] path)
    {
        var list = new List<XElement>();

        if (path.Length >= 1)
        {
            foreach (var element in container.Elements(path[0]))
            {
                list.AddRange(FindElements(element, path[1..]));
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