using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blogrolling.Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Comment("数据源")]
public class DataSource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("源ID")]
    public int Id { get; set; }
    
    [Required]
    [Comment("数据源状态")]
    public required DataSourceStatus Status { get; set; } = DataSourceStatus.Ok;

    [Required]
    [Comment("数据源类型")]
    public required DataSourceType Type { get; set; }
    
    [Required]
    [Comment("链接")]
    public required string Link { get; set; }
    
    [Comment("上次Feed更新时间")]
    public DateTime? LastUpdateTime { get; set; }
    
    [Comment("上次获取时间")]
    public DateTime? PrevFetchTime { get; set; }
    
    [Comment("下次获取时间")]
    public DateTime? NextFetchTime { get; set; }

    #region Foreign Keys
    
    [Required]
    [Comment("博客ID")]
    public int BlogId { get; set; }
    
    public virtual Blog Blog { get; set; }

    #endregion
}