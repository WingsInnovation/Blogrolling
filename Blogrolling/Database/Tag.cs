using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[Index(nameof(Name), IsUnique = true)]
[Comment("标签")]
public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("标签Id")]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public virtual IEnumerable<PostTag> PostTags { get; set; }
}