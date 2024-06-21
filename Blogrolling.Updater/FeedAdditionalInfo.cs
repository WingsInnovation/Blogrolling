namespace Blogrolling.Updater;

public class FeedAdditionalInfo
{
    // public string? Guid { get; set; }
    public string? Link { get; set; }

    public DateTime? SyUpdateFrequency { get; set; }
    public DateTime NextFetchTime { get; set; } = DateTime.Now.AddHours(12);
}