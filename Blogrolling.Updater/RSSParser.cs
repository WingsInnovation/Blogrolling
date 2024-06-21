using CodeHollow.FeedReader;

namespace Blogrolling.Updater;

public class RSSParser
{
    public static Feed? Fetch(string link)
    {
        try
        {
            using var client = new HttpClient();
            // client.Timeout = TimeSpan.FromSeconds(ConfigHelper.GetTimeout());
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