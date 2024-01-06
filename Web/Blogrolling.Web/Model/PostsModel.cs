using Blogrolling.Database;

namespace Blogrolling.Web.Model;

public class PostsModel
{
    public int Total { get; set; }
    public int Limit { get; set; }
    public int Count { get; set; }
    public int Page { get; set; }
    public List<Post> Posts { get; set; }
}