using System.Security.Cryptography;
using System.Text;
using Blogrolling.Database;
using Blogrolling.Utilities;
using CodeHollow.FeedReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blogrolling.Data.RSS;

public class Helper
{
    public static bool IsUrl(string str)
    {
        return Uri.TryCreate(str, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    public static BlogrollingContext GetContext()
    {
        var connectionString = ConfigHelper.GetConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Please place the ConnectionString into '%USER_PROFILE%/.config/blogrolling/connectionString'.");
        }
    
        var optionsBuilder = new DbContextOptionsBuilder<BlogrollingContext>();
        optionsBuilder.UseLazyLoadingProxies()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        if (ConfigHelper.IsDebug())
        {
            Console.WriteLine("Debug mode enabled!");
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        }
        
        return new BlogrollingContext(optionsBuilder.Options);
    }

    public static bool IsRSS(FeedType type)
    {
        return type is FeedType.Rss or FeedType.Rss_2_0 or FeedType.Rss_1_0 or FeedType.Rss_0_92 or FeedType.Rss_0_91;
    }

    public static DateTime? ToUpdateDateTime(string period, int frequency)
    {
        return period switch
        {
            "hourly" => new DateTime(1, 1, 1, frequency, 0, 0),
            "daily" => new DateTime(1, 1, frequency, 0, 0, 0),
            "weekly" => new DateTime(1, 1, frequency * 7, 0, 0, 0),
            "monthly" => new DateTime(1, frequency, 1, 0, 0, 0),
            "yearly" => new DateTime(frequency, 1, 1, 0, 0, 0),
            _ => null   // Period out of range, but a recover-able problem.
        };
    }
    
    public static DateTime? ToNextUpdateTime(DateTime frequency, DateTime updateBase)
    {
        var now = DateTime.Now;
        
        while (updateBase < now)
        {
            updateBase = updateBase.AddTicks(frequency.Ticks);
        }

        return updateBase;
    }

    public static string HashSha512(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        var hashed = SHA512.HashData(bytes);

        var builder = new StringBuilder(128);
        foreach (var b in hashed)
        {
            builder.Append(b.ToString("X2"));
        }
        return builder.ToString();
    }

    public static string GetBaseUri(string fullPath, string value = "")
    {
        if (value == "")
        {
            return fullPath.Replace(new Uri(fullPath).PathAndQuery, "");
        }
        
        if (value == "//")
        {
            return new Uri(new Uri(fullPath), "/").AbsoluteUri;
        }

        return new Uri(new Uri(fullPath), value).AbsoluteUri;
    }
}