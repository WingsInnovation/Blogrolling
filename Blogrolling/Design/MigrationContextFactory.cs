using Blogrolling.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Blogrolling.Design;

public class MigrationContextFactory : IDesignTimeDbContextFactory<BlogrollingContext>
{
    public BlogrollingContext CreateDbContext(string[] args)
    {
        if (args.Length != 1)
        {
            throw new Exception("Argument must be a MySQL connection string. Eg: dotnet ef database update -- \"<Connection string.>\". ");
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<BlogrollingContext>();
        optionsBuilder.UseMySql(ServerVersion.AutoDetect(args[0]));
        return new BlogrollingContext(optionsBuilder.Options);
    }
}