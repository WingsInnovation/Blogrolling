using Blogrolling.Database.Conversions;
using Blogrolling.Database.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blogrolling.Database;

public class BlogrollingContext(DbContextOptions<BlogrollingContext> options) : DbContext(options)
{
    public DbSet<Blog> Blogs { get; set; }
    
    public DbSet<DataSource> DataSources { get; set; }
    
    public DbSet<Post> Posts { get; set; }
    
    public DbSet<Tag> Tags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Blog>()
            .HasMany(b => b.Sources)
            .WithOne(s => s.Blog)
            .HasForeignKey(s => s.BlogId);

        builder.Entity<Blog>()
            .HasMany(b => b.Posts)
            .WithOne(p => p.Blog)
            .HasForeignKey(p => p.BlogId);

        builder.Entity<Post>()
            .HasMany(p => p.Tags)
            .WithMany(t => t.Posts)
            .UsingEntity<PostTag>();

        builder.Entity<Post>()
            .Property(p => p.PublishTime)
            .HasConversion(new DateTimeToISO8601Converter());
        
        builder.Entity<Post>()
            .Property(p => p.UpdateTime)
            .HasConversion(new DateTimeToISO8601Converter());

        builder.Entity<Tag>()
            .HasOne(t => t.Blog)
            .WithMany(b => b.Tags)
            .HasForeignKey(t => t.BlogId);
        
        builder.Entity<DataSource>()
            .Property(d => d.Type)
            .HasConversion(new EnumToStringConverter<DataSourceType>());

        builder.Entity<DataSource>()
            .Property(d => d.Status)
            .HasConversion(new EnumToStringConverter<DataSourceStatus>());

        builder.Entity<DataSource>()
            .Property(d => d.LastUpdateTime)
            .HasConversion(new DateTimeToISO8601Converter());
        
        builder.Entity<DataSource>()
            .Property(d => d.PrevFetchTime)
            .HasConversion(new DateTimeToISO8601Converter());
        
        builder.Entity<DataSource>()
            .Property(d => d.NextFetchTime)
            .HasConversion(new DateTimeToISO8601Converter());
    }
}