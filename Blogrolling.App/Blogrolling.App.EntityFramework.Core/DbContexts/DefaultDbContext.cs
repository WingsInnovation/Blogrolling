using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.App.EntityFramework.Core;

[AppDbContext("Blogrolling.App", DbProvider.Sqlite)]
public class DefaultDbContext : AppDbContext<DefaultDbContext>
{
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
    {
    }
}