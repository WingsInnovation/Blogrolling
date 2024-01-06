using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Index(nameof(Guid), nameof(BlogId), IsUnique = true)]
[Comment("标签")]
public class Tag
{
    [JsonIgnore]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("标签Id")]
    public int Id { get; set; }
    
    [Required]
    [Comment("标签名")]
    public string Name { get; set; }
    
    [JsonIgnore]
    [Required]
    [Comment("标签Guid的Hash")]
    public string Guid { get; set; }
    
    [Comment("标签链接")]
    public string? Link { get; set; }

    #region Foreign Keys

    [JsonIgnore]
    [Required]
    [Comment("博客Id")]
    public int BlogId { get; set; }
    
    [JsonIgnore]
    public virtual Blog Blog { get; set; }
    
    [JsonIgnore]
    public virtual IEnumerable<Post> Posts { get; set; }

    #endregion
}