using System.Xml.Linq;

namespace Blogrolling.Data.RSS;

public static class XmlHelper
{
    public static XName SyNode(string node)
    {
        return XName.Get(node, "http://purl.org/rss/1.0/modules/syndication/");
    }
    
    public static XName AtomNode(string node)
    {
        return XName.Get(node, "http://www.w3.org/2005/Atom");
    }
    
    public static XName DcNode(string node)
    {
        return XName.Get(node, "http://purl.org/dc/elements/1.1/");
    }
    
    public static IEnumerable<XElement> FindElements(XContainer container, params XName[] path)
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
}