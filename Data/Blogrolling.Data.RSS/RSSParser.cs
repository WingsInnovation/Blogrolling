using CodeHollow.FeedReader;

namespace Blogrolling.Data.RSS;

public class RSSParser
{
    public static Feed? Fetch(string link)
    {
        try
        {
            using var client = new HttpClient();
            var text = client.GetStringAsync(link).Result;
            return FeedReader.ReadFromString(text);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}