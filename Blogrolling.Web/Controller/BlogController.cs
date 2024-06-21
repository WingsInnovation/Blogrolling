using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Blogrolling.Database;
using Blogrolling.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Web.Controller;

[ApiController]
[Route("/api/blogrolling")]
public class BlogController(BlogrollingContext context) : ControllerBase
{
    private BlogrollingContext Db { get; } = context;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderType
    {
        [EnumMember(Value = "time")]
        TimeAsc = 0,
        
        [EnumMember(Value = "time-desc")]
        Time = 1,
        
        [EnumMember(Value = "alphabet")]
        Alphabet = 2,
        
        [EnumMember(Value = "alphabet-desc")]
        AlphabetDesc = 3,
        
        [EnumMember(Value = "relativity")]
        Relativity = 4,
        
        [EnumMember(Value = "relativity-desc")]
        RelativityDesc = 5,
    }
    
    [HttpGet]
    [Route("posts")]
    [ProducesResponseType(200)]
    public IActionResult GetPosts(
        [FromQuery(Name = "tag")] string[] tags,
        [FromQuery(Name = "search")] string search = "",
        [FromQuery(Name = "order")] OrderType order = OrderType.Time,
        [FromQuery(Name = "limit")] int limit = 10, 
        [FromQuery(Name = "page")] int page = 1)
    {
        IQueryable<Post> posts = Db.Posts;
        
        if (tags.Length >= 1)
        {
            if (tags.Length >= 5)
            {
                tags = tags[..5];
            }
            
            var tagsList = Db.Tags
                .Where(t => tags.Contains(t.Name))
                .ToList();
            
            posts = posts.Include(p => p.Tags)
                .Where(p => p.Tags.Any(t => tagsList.Contains(t)));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            posts = posts.Where(p => p.Title.Contains(search)
                                     || (p.Description != null && p.Description.Contains(search)));
        }
        
        posts = order switch
        {
            OrderType.TimeAsc => posts.OrderBy(p => p.PublishTime),
            OrderType.Time => posts.OrderByDescending(p => p.PublishTime),
            OrderType.Alphabet => posts.OrderBy(p => p.Title),
            OrderType.AlphabetDesc => posts.OrderByDescending(p => p.Title),
            OrderType.Relativity => posts,  // Todo: How to impl?
            OrderType.RelativityDesc => posts,  // Todo: How to impl?
            _ => throw new ArgumentOutOfRangeException(nameof(order), order, null)
        };

        var total = posts.Count();
        var totalPages = total / limit + 1;
        
        limit = Math.Clamp(limit, 5, 25);
        page = Math.Clamp(page, 1, totalPages);

        var result = posts.Skip((page - 1) * limit)
            .Take(limit)
            .ToList();

        var model = new PostsModel
        {
            Total = total,
            Limit = limit,
            Page = page,
            Posts = result,
            Count = result.Count
        };

        return Ok(model);
    }
}