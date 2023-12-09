using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Comment("博客数据源")]
public class Blog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("博客ID")]
    public int Id { get; set; }
    
    [Required]
    [Comment("博客名称")]
    public string Name { get; set; }
    
    // XXX: qyl27 - Do NOT create a feature request for now?
    [Comment("博客简介")]
    public string? Description { get; set; }
    
    [Comment("博客作者")]
    public string? Author { get; set; }
    
    [Required]
    [Comment("博客链接")]
    public string Link { get; set; }
    
    [Required]
    [Comment("博客状态")]
    public BlogStatus Status { get; set; }
    
    [Required]
    [Comment("博客标志")]
    public string Guid { get; set; }

    #region Feed

    [Required]
    [Comment("Feed")]
    public string Feed { get; set; }
    
    [Required]
    [Comment("上次Feed更新时间")]
    public long FeedPrevUpdate { get; set; }
    
    [Required]
    [Comment("下次Feed更新时间")]
    public long FeedNextUpdate { get; set; }

    #endregion
    
    public virtual IEnumerable<Post> Posts { get; set; }
    
    public enum BlogStatus
    {
        Ok = 0,
        Invalid = 1,
        Disabled = 2,
    }
}