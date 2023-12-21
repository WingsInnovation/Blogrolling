using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[PrimaryKey(nameof(PostId), nameof(TagId))]
[Comment("文章标签")]
public class PostTag
{
    [Comment("文章ID")]
    public int PostId { get; set; }
    
    [Comment("标签ID")]
    public int TagId { get; set; }
}