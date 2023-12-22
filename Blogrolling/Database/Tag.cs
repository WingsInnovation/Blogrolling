using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Index(nameof(Guid), nameof(BlogId), nameof(Link), IsUnique = true)]
[Comment("标签")]
public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("标签Id")]
    public int Id { get; set; }
    
    [Required]
    [Comment("标签名")]
    public string Name { get; set; }
    
    [Required]
    [Comment("标签Guid")]
    public string Guid { get; set; }
    
    [Comment("标签链接")]
    public string? Link { get; set; }

    #region Foreign Keys

    [Required]
    [Comment("博客Id")]
    public int BlogId { get; set; }
    
    public virtual Blog Blog { get; set; }
    
    public virtual IEnumerable<Post> Posts { get; set; }

    #endregion
}