using System.Text.Json.Serialization;
using Blogrolling.Database;
using Blogrolling.Utilities;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders()
    .SetMinimumLevel(LogLevel.Debug)
    .AddNLogWeb();

builder.Services.AddDbContext<BlogrollingContext>(context =>
{
    context.UseMySql(ConfigHelper.GetConnectionString(), ServerVersion.AutoDetect(ConfigHelper.GetConnectionString()))
        .UseLazyLoadingProxies();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseRouting();

app.MapControllers();

app.Run();
