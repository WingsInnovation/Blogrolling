using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

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
    public required string Title { get; set; }
    
    [Comment("文章简介")]
    public string? Description { get; set; }
    
    [Comment("文章作者")]
    public string? Author { get; set; }
    
    [Required]
    [Comment("链接")]
    public required string Link { get; set; }
    
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
    
    [JsonIgnore]
    public virtual IEnumerable<Tag> Tags { get; set; }

    #endregion
}