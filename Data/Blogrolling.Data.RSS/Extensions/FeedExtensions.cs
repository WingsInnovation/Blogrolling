using System.Globalization;
using CodeHollow.FeedReader;

namespace Blogrolling.Data.RSS.Extensions;

public static class FeedExtensions
{
    public static FeedAdditionalInfo GetAdditionalInfo(this Feed feed, string link)
    {
        var document = feed.SpecificFeed.Element.Document!;
        var info = new FeedAdditionalInfo();
        
        switch (feed.Type)
        {
            case FeedType.Rss or FeedType.Rss_2_0 or FeedType.Rss_1_0 or FeedType.Rss_0_92 or FeedType.Rss_0_91 or FeedType.MediaRss:
            {
                var blogLink = XmlHelper.FindElements(document, "rss", "channel", "link").FirstOrDefault();
                if (blogLink is not null)
                {
                    info.Link = Helper.GetBaseUri(link, blogLink.Value);
                }

                break;
            }
            case FeedType.Atom:
            {
                var blogLinks = XmlHelper.FindElements(document, XmlHelper.AtomNode("feed"), XmlHelper.AtomNode("link"));
                foreach (var blogLink in blogLinks.Where(blogLink => blogLink.Attribute("rel") == null ||
                                                                     (blogLink.Attribute("rel") != null && blogLink.Attribute("rel")!.Value == "alternate")))
                {
                    info.Link = Helper.GetBaseUri(link, blogLink.Attribute("href")!.Value);
                    break;
                }

                if (info.Link is null)
                {
                    info.Link = Helper.GetBaseUri(link);
                    break;
                }

                break;
            }
        }
        
        var updatePeriod = XmlHelper.FindElements(document, "rss", "channel", XmlHelper.SyNode("updatePeriod")).FirstOrDefault();
        if (updatePeriod is not null)
        {
            var updateFrequency = XmlHelper.FindElements(document, "rss", "channel", XmlHelper.SyNode("updateFrequency")).FirstOrDefault();
            if (updateFrequency is not null)
            {
                if (int.TryParse(updateFrequency.Value, out var value))
                {
                    info.SyUpdateFrequency = Helper.ToUpdateDateTime(updatePeriod.Value.Trim(), value);

                    if (info.SyUpdateFrequency is not null)
                    {
                        var updateBase = XmlHelper.FindElements(document, "rss", "channel", XmlHelper.SyNode("updateBase")).FirstOrDefault();
                        if (updateBase is not null)
                        {
                            if (DateTime.TryParse(updateBase.Value, CultureInfo.InvariantCulture, out var baseDateTime))
                            {
                                var next = Helper.ToNextUpdateTime(info.SyUpdateFrequency.Value, baseDateTime);
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

        return info;
    }
}