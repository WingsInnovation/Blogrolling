using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

[PrimaryKey(nameof(PostId), nameof(TagId))]
[Comment("文章标签")]
public class PostTag
{
    [JsonIgnore]
    [Comment("文章ID")]
    public int PostId { get; set; }
    
    [JsonIgnore]
    [Comment("标签ID")]
    public int TagId { get; set; }
}