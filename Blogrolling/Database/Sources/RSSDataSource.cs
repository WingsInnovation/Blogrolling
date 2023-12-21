using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database.Sources;

public class RSSDataSource : DataSource
{
    [Comment("上次Feed更新时间")]
    public DateTime? LastUpdateTime { get; set; }
    
    [Comment("上次获取时间")]
    public DateTime? PrevFetchTime { get; set; }
    
    /// <summary>
    /// A DateTime after 0-0-0 0:0:0.
    /// </summary>
    [Comment("更新频率")]
    public DateTime? UpdateFrequency { get; set; }
    
    [Comment("下次获取时间")]
    public DateTime? NextFetchTime { get; set; }
}