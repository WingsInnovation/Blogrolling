using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Comment("博客")]
public class Blog
{
    [JsonIgnore]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("博客ID")]
    public int Id { get; set; }
    
    [Required]
    [Comment("博客名称")]
    public required string Name { get; set; }
    
    [Comment("博主")]
    public string? Owner { get; set; }
    
    [Required]
    [Comment("博客链接")]
    public required string Link { get; set; }

    #region Foreign Keys
    
    [JsonIgnore]
    public virtual IEnumerable<DataSource> Sources { get; set; }
    
    [JsonIgnore]
    public virtual IEnumerable<Post> Posts { get; set; }
    
    [JsonIgnore]
    public virtual IEnumerable<Tag> Tags { get; set; }

    #endregion
}