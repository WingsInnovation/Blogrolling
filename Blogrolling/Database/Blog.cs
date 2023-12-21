using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blogrolling.Database.Sources;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Index(nameof(Guid), IsUnique = true)]
[Comment("博客")]
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
    [Comment("博客标志")]
    public string Guid { get; set; }

    #region Foreign Keys
    
    [Comment("数据源ID")]
    public int? SourceId { get; set; }
    
    public virtual DataSource? Source { get; set; }
    
    public virtual IEnumerable<Post> Posts { get; set; }

    #endregion
}