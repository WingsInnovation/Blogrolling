using Blogrolling.Database.Conversions;
using Blogrolling.Database.Sources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blogrolling.Database;

public class BlogrollingContext(DbContextOptions<BlogrollingContext> options) : DbContext(options)
{
    public DbSet<DataSource> DataSources { get; }
    
    public DbSet<RSSDataSource> RSSDataSources { get; }
    
    public DbSet<Blog> Blogs { get; }
    
    public DbSet<Post> Posts { get; }
    
    public DbSet<Tag> Tags { get; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Blog>()
            .HasOne(b => b.Source)
            .WithOne(s => s.Blog)
            .HasForeignKey<Blog>(b => b.SourceId);

        builder.Entity<Blog>()
            .HasMany(b => b.Posts)
            .WithOne(p => p.Blog)
            .HasForeignKey(p => p.BlogId);

        builder.Entity<Post>()
            .HasMany(p => p.Tags)
            .WithMany(t => t.Posts)
            .UsingEntity<PostTag>();

        builder.Entity<DataSource>()
            .HasDiscriminator(d => d.Type)
            .HasValue<DataSource>(DataSourceType.Manual)
            .HasValue<RSSDataSource>(DataSourceType.RSS)
            .IsComplete(false);

        builder.Entity<DataSource>()
            .Property(d => d.Type)
            .HasConversion(new EnumToStringConverter<DataSourceType>());

        builder.Entity<DataSource>()
            .Property(d => d.Status)
            .HasConversion(new EnumToStringConverter<DataSourceStatus>());

        builder.Entity<RSSDataSource>()
            .Property(d => d.LastUpdateTime)
            .HasConversion(new DateTimeToISO8601Converter());
        
        builder.Entity<RSSDataSource>()
            .Property(d => d.UpdateFrequency)
            .HasConversion(new DateTimeToISO8601Converter());
        
        builder.Entity<RSSDataSource>()
            .Property(d => d.PrevFetchTime)
            .HasConversion(new DateTimeToISO8601Converter());
        
        builder.Entity<RSSDataSource>()
            .Property(d => d.NextFetchTime)
            .HasConversion(new DateTimeToISO8601Converter());
    }
}