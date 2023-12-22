using System.Globalization;
using CodeHollow.FeedReader;

namespace Blogrolling.Data.RSS.Extensions;

public static class FeedItemExtensions
{
    public static DateTime? GetUpdateTime(this FeedItem item, FeedType type)
    {
        var element = item.SpecificItem.Element;

        if (type == FeedType.Atom)
        {
            var updated = XmlHelper.FindElements(element, XmlHelper.AtomNode("updated")).FirstOrDefault();
            if (DateTime.TryParse(updated?.Value, CultureInfo.InvariantCulture, out var time))
            {
                return time;
            }
        }
        
        return null;
    }

    public static string? GetAuthor(this FeedItem item, FeedType type)
    {
        var element = item.SpecificItem.Element;

        if (type == FeedType.Atom)
        {
            if (element.Document == null)
            {
                return item.Author;
            }

            var author = XmlHelper.FindElements(element.Document, XmlHelper.AtomNode("feed"),
                XmlHelper.AtomNode("author"), XmlHelper.AtomNode("name")).FirstOrDefault();
            return author != null ? author.Value : item.Author;
        }

        if (Helper.IsRSS(type))
        {
            var author = XmlHelper.FindElements(element, XmlHelper.DcNode("creator")).FirstOrDefault();
            return author != null ? author.Value : item.Author;
        }

        return item.Author;
    }
    
    public static ICollection<FeedTag> GetCategories(this FeedItem item, FeedType type)
    {
        var element = item.SpecificItem.Element;

        if (type == FeedType.Atom)
        {
            var list = new List<FeedTag>();

            var categories = XmlHelper.FindElements(element, XmlHelper.AtomNode("category"));
            foreach (var category in categories)
            {
                var link = category.Attribute("scheme");
                var guid = category.Attribute("term");
                var name = category.Attribute("label");
                list.Add(new FeedTag(name!.Value, link?.Value, guid?.Value));
            }
            
            return list;
        }

        return item.Categories.Select(c => new FeedTag(c)).ToList();
    }
    
    public class FeedTag(string name, string? link = null, string? guid = null)
    {
        public string Name { get; } = name;

        public string? Link { get; } = link;

        public string? Guid { get; } = guid;
    }
}