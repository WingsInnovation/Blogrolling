using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database;

public class BlogrollingContext(DbContextOptions<BlogrollingContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Blog>()
            .Property(b => b.Status)
            .HasConversion(p => p.ToString(), 
                p => Enum.Parse<Blog.BlogStatus>(p));

        builder.Entity<Blog>()
            .HasMany<Post>(b => b.Posts)
            .WithOne(p => p.Blog)
            .HasForeignKey(p => p.BlogId);

        builder.Entity<PostTag>()
            .HasOne(p => p.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(p => p.PostId);

        builder.Entity<PostTag>()
            .HasOne(p => p.Tag)
            .WithMany(t => t.PostTags)
            .HasForeignKey(p => p.TagId);
    }
}