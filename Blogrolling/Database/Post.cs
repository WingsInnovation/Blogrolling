using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Index(nameof(Guid), IsUnique = true)]
[Comment("博客文章")]
public class Post
{
    [JsonIgnore]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("文章ID")]
    public int Id { get; set; }
    
    [Required]
    [Comment("文章标题")]
    public string Title { get; set; }
    
    [Comment("文章介绍")]
    public string? Description { get; set; }
    
    [Comment("文章作者")]
    public string? Author { get; set; }
    
    [Required]
    [Comment("链接")]
    public string Link { get; set; }
    
    [JsonIgnore]
    [Required]
    [Comment("文章GUID的Hash")]
    public string Guid { get; set; }
    
    [Required]
    [Comment("发布时间")]
    public DateTime PublishTime { get; set; }
    
    [Comment("更新时间")]
    public DateTime? UpdateTime { get; set; }

    #region Foreign Keys

    [JsonIgnore]
    [Required]
    [Comment("博客ID")]
    public int BlogId { get; set; }
    
    [JsonIgnore]
    public virtual Blog Blog { get; set; }
    
    public virtual IEnumerable<Tag> Tags { get; set; }

    #endregion
}