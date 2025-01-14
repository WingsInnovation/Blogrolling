using System.Text.Json.Serialization;
using Blogrolling.Config;
using Blogrolling.Database;
using Blogrolling.Service;
using Blogrolling.Service.Impl;
using Coravel;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders()
    .SetMinimumLevel(LogLevel.Debug)
    .AddNLogWeb();

var config = new ConfigManager();
builder.Services.AddSingleton(config);
builder.Services.AddDbContext<BlogrollingContext>(context =>
{
    context.UseMySql(config.GetConnectionString(), ServerVersion.AutoDetect(config.GetConnectionString()))
        .UseLazyLoadingProxies();
});
builder.Services.AddSingleton<IPollingService, PollingService>();

builder.Services.AddScheduler();

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

app.UseRouting();
app.MapControllers();

var db = app.Services.GetService<BlogrollingContext>();

app.Run();
