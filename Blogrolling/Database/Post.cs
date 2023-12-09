using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Index(nameof(Guid), IsUnique = true)]
[Comment("博客文章")]
public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("文章ID")]
    public int Id { get; set; }
    
    [Required]
    [Comment("文章标题")]
    public string Title { get; set; }
    
    [Required]
    [Comment("文章介绍")]
    public string Description { get; set; }
    
    [Required]
    [Comment("文章GUID")]
    public string Guid { get; set; }

    #region Foreign Keys

    [Required]
    [Comment("博客ID")]
    public int BlogId { get; set; }
    
    public virtual Blog Blog { get; set; }
    
    public virtual IEnumerable<PostTag> PostTags { get; set; }

    #endregion
}