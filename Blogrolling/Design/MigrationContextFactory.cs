using Blogrolling.Config;
using Blogrolling.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Blogrolling.Design;

public class MigrationContextFactory : IDesignTimeDbContextFactory<BlogrollingContext>
{
    public BlogrollingContext CreateDbContext(string[] args)
    {
        var config = new ConfigManager();
        var connectionString = args.Length != 1 ? config.GetConnectionString() : args[0];

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Provide a MySQL connection string. Eg: dotnet ef database update -- \"<Connection string.>\". ");
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<BlogrollingContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new BlogrollingContext(optionsBuilder.Options);
    }
}